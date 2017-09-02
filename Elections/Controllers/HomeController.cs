using Elections.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Elections.Controllers
{
	public class HomeController : Controller
	{
		private readonly ElectionContext _context;

		public HomeController(ElectionContext context)
		{
			_context = context;
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

			//var md5 = System.Security.Cryptography.MD5.Create();
			//var hashArray = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(email));

			//var hash = System.Text.Encoding.UTF8.GetString(md5.Hash);

			var voter = new Voter
			{
				Email = email,
				Code = email.MD5()
			};

			_context.Add(voter);
			await _context.SaveChangesAsync();

			return Json(voter);
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

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[Route("/vote")]
		public IActionResult Vote()
		{
			var code = HttpContext.Session.GetString("code");
			if (string.IsNullOrEmpty(code)) return Redirect("/logout");

			return Ok(code);
		}

		[Route("/logout")]
		public IActionResult Logout()
		{
			HttpContext.Session.SetString("code", string.Empty);
			return Redirect("/");
		}
	}
}
