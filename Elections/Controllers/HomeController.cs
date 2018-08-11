using Elections.Models;
using Elections.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Elections.Options;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Elections.Controllers
{
	public class HomeController : Controller
	{
		private readonly ElectionContext _context;
		private readonly Mailer _mailer;

		public const bool VOTING_CLOSED = true;
		public const int LAST_VOTE_ID = 91;

		public HomeController(ElectionContext context, Mailer mailer)
		{
			_context = context;
			_mailer = mailer;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Route("/Process")]
		public IActionResult Process()
		{
			return View("Process");
		}

		[HttpPost]
		[Route("/signup")]
		public async Task<IActionResult> SignUp([FromForm]string email)
		{
			email = email.Trim();

			if (_context.Voters.Any(x => x.Email == email))
			{
				return Ok("You've already signed up");
			}
			
			var code = email.MD5();
			var voter = new Voter
			{
				Email = email,
				Code = code
			};

			_context.Add(voter);
			await _context.SaveChangesAsync();
			
			return Ok(_mailer.SendConfirmationMail(email, code));
		}
		
		[HttpPost]
		[Route("/login")]
		public IActionResult Login([FromForm(Name = "code")]string code)
		{
			code = code.Trim();

			if (!_context.Voters.Any(x => x.Code == code))
			{
				return Ok("No signup has been requested with this code!");
			}

			HttpContext.Session.SetString("code", code);
			return Redirect("/vote");
		}


		[HttpPost]
		[Route("/submit")]
		public async Task<IActionResult> Submit()
		{
			var voter = _context.Voters.SingleOrDefault(v => v.Code == HttpContext.Session.GetString("code"));
			if (voter == null) return BadRequest("Bad Request");

			if (_context.Votes.Include(x => x.Voter).Any(x => x.Voter == voter))
				return BadRequest($"It seems you've already tried to vote once, {voter.Email}; you can't vote again. If you are not {voter.Email}, please go to the /logout endpoint.");

			var candidates = _context.Candidates.ToList();
			var votes = new Dictionary<Candidate, bool>();

			foreach(var c in _context.Candidates)
			{
				votes.Add(c, false);
			}

			foreach(var a in Request.Form)
			{
				var cand = _context.Candidates.SingleOrDefault(x => x.Id == int.Parse(a.Key));
				if (cand == null) continue;
				votes[cand] = a.Value == "on";
			}

			List<Vote> votes2 = new List<Models.Vote>();
			foreach(var v in votes)
			{
				votes2.Add(new Vote
				{
					Candidate = v.Key,
					For = v.Value,
					Voter = voter
				});
			}
			_context.AddRange(votes2);
			await _context.SaveChangesAsync();

			return Redirect("/logout");
		}


		[HttpGet]
		[Route("/login")]
		public IActionResult LoginGet([FromQuery(Name = "code")]string code)
		{
			code = code.Trim();

			if (!_context.Voters.Any(x => x.Code == code))
			{
				return Ok("No signup has been requested with this code!");
			}

			HttpContext.Session.SetString("code", code);
			return Redirect("/vote");
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[Route("/vote")]
		public IActionResult Vote()
		{
			if (VOTING_CLOSED) return Redirect("/");

			var code = HttpContext.Session.GetString("code");
			if (string.IsNullOrEmpty(code)) return Redirect("/logout");

			ViewBag.Code = code;

			var model = _context.Candidates.GroupBy(x => x.Position).Select(x => new PositionalGrouping
			{
				Candidates = x.OrderBy(c => c.Name).ToList(),
				MaxCandidates = x.Key == Position.Committee ? 15 : 1,
				Position = x.Key
			});

			return View(model);
		}

		[Route("/report")]
		public IActionResult Report()
		{
			var group = _context.Votes
				.Include(x => x.Candidate)
				.GroupBy(x => x.Candidate);

			Dictionary<Candidate, Tuple<int, int>> score = new Dictionary<Candidate, Tuple<int, int>>();
		
			foreach(var g in group)
			{
				g.Count(x => x.For);
				score.Add(g.Key, new Tuple<int, int>(g.Count(x => x.For), g.Count(x => !x.For)));
			}

			return View(score
				.Select(x => new ReportObject(x))
				.OrderByDescending(x => x.For)
				.OrderBy(x => x.Position));
		}

		[Route("/timedreport")]
		public IActionResult TimedReport()
		{
			var group = _context.Votes
				.Include(x => x.Candidate)
				.Include(x => x.Voter)
				.Where(x => x.Voter.Id < LAST_VOTE_ID)
				.GroupBy(x => x.Candidate);

			Dictionary<Candidate, Tuple<int, int>> score = new Dictionary<Candidate, Tuple<int, int>>();

			foreach (var g in group)
			{
				g.Count(x => x.For);
				score.Add(g.Key, new Tuple<int, int>(g.Count(x => x.For), g.Count(x => !x.For)));
			}

			return View("Report", score
				.Select(x => new ReportObject(x))
				.OrderByDescending(x => x.For)
				.OrderBy(x => x.Position));
		}

		[Route("/timedcleanreport")]
		public IActionResult TimedCleanReport()
		{
			var group = _context.Votes
				.Include(x => x.Candidate)
				.Include(x => x.Voter)
				.Where(x => x.Voter.Id <= LAST_VOTE_ID)
				.GroupBy(x => x.Voter)
				.Where(g => g.Count(x => x.For) > 1)
				.SelectMany(g => g)
				.GroupBy(x => x.Candidate);

			var score = new Dictionary<Candidate, Tuple<int, int>>();

			foreach (var g in group)
			{
				g.Count(x => x.For);
				score.Add(g.Key, new Tuple<int, int>(g.Count(x => x.For), g.Count(x => !x.For)));
			}

			return View("Report", score
				.Select(x => new ReportObject (x))
				.OrderByDescending(x => x.For)
				.OrderBy(x => x.Position));
		}


		[Route("/cleanreport")]
		public IActionResult CleanReport()
		{
			var groups = _context.Votes
				.Include(x => x.Candidate)
				.Include(x => x.Voter)
				.GroupBy(x => x.Voter)
				.Where(g => g.Count(x => x.For) > 1)
				.SelectMany(g => g).GroupBy(x => x.Candidate);

			Dictionary<Candidate, Tuple<int, int>> score = new Dictionary<Candidate, Tuple<int, int>>();

			foreach (var group in groups)
			{
				group.Count(x => x.For);
				score.Add(group.Key, new Tuple<int, int>(group.Count(x => x.For), group.Count(x => !x.For)));
			}

			return View("Report", score
				.Select(x => new ReportObject(x))
				.OrderByDescending(x => x.For)
				.OrderBy(x => x.Position));
		}

		[Route("/uncleanreport")]
		public IActionResult UncleanReport()
		{
			var groups = _context.Votes
				.Include(x => x.Candidate)
				.Include(x => x.Voter)
				.GroupBy(x => x.Voter)
				.Where(g => g.Count(x => x.For) <= 1)
				.SelectMany(g => g)
				.GroupBy(x => x.Candidate);

			Dictionary<Candidate, Tuple<int, int>> score = new Dictionary<Candidate, Tuple<int, int>>();

			foreach (var group in groups)
			{
				group.Count(x => x.For);
				score.Add(group.Key, new Tuple<int, int>(group.Count(x => x.For), group.Count(x => !x.For)));
			}

			return View("Report", score
				.Select(x => new ReportObject(x))
				.OrderByDescending(x => x.For)
				.OrderBy(x => x.Position));
		}

		public class ReportObject
		{
			public ReportObject(KeyValuePair<Candidate, Tuple<int, int>> kvp)
			{
				Name = kvp.Key.Name;
				Position = kvp.Key.Position.ToString();
				For = kvp.Value.Item1;
				Against = kvp.Value.Item2;
			}

			public string Name { get; set; }
			public string Position { get; set; }
			public int For { get; set; }
			public int Against { get; set; }
		}
		
		[Route("/logout")]
		public IActionResult Logout()
		{
			HttpContext.Session.SetString("code", string.Empty);
			return Redirect("/");
		}
	}
}
