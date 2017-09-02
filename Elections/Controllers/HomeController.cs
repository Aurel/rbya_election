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

			//var md5 = System.Security.Cryptography.MD5.Create();
			//var hashArray = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(email));

			//var hash = System.Text.Encoding.UTF8.GetString(md5.Hash);
			var code = email.MD5();
			var voter = new Voter
			{
				Email = email,
				Code = code
			};

			_context.Add(voter);
			await _context.SaveChangesAsync();
			
			_mailer.SendConfirmationMail(email, code);

			return Json(voter);
		}

		[Route("/test")]
		public string Test()
		{
			var options = HttpContext.RequestServices.GetService<IOptions<GmailOptions>>();
			return options.Value.Email;
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
