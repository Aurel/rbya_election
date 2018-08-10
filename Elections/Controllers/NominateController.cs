using Elections.Models;
using Elections.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elections.Controllers
{
	public class NominateController : Controller
	{
		private readonly ElectionContext _context;
		private readonly Mailer _mailer;

		public NominateController(ElectionContext context, Mailer mailer)
		{
			_context = context;
			_mailer = mailer;
		}

		[HttpGet]
		[Route("/nominate")]
		public IActionResult Nominate()
		{
			return View("Nominate");
		}

	}
}
