﻿using Elections.Models;
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

		public HomeController(ElectionContext context, Mailer mailer)
		{
			_context = context;
			_mailer = mailer;
		}

		public IActionResult Index()
		{
			return View();
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

		public IActionResult Report()
		{
			var group = _context.Votes.Include(x => x.Candidate).GroupBy(x => x.Candidate);

			Dictionary<Candidate, Tuple<int, int>> score = new Dictionary<Candidate, Tuple<int, int>>();
		
			foreach(var g in group)
			{
				g.Count(x => x.For);
				score.Add(g.Key, new Tuple<int, int>(g.Count(x => x.For), g.Count(x => !x.For)));
			}

			return View(score.Select(x => new ReportObject { Name = x.Key.Name, Position = x.Key.Position.ToString(), For = x.Value.Item1, Against = x.Value.Item2 }).OrderBy(x => x.Position));
		}

		public class ReportObject
		{
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
