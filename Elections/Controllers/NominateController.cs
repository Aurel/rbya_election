using Elections.Models;
using Elections.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
			Candidate c = new Candidate();
			return View("Nominate", c);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Candidate candidate)
		{
			if (ModelState.IsValid)
			{
				_context.Add(candidate);
				await _context.SaveChangesAsync();
				return Redirect("/Candidates"); //TODO: Thank you page.
			}

			return RedirectToAction(nameof(Nominate));
		}

		public IActionResult Thanks()
		{
			return View();
		}

	}
}
