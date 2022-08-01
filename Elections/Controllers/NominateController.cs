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
		private readonly ElectionDecider _decider;

		public NominateController(ElectionContext context, Mailer mailer, ElectionDecider decider)
		{
			_context = context;
			_mailer = mailer;
			_decider = decider;
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
			candidate.Guid = System.Guid.NewGuid();

			if (ModelState.IsValid)
			{
				var currentElection = _decider.GetCurrentElection();
				candidate.ElectionYear = currentElection.Year;
				_context.Add(candidate);

				await _context.SaveChangesAsync();

				_mailer.SendCandidateConfirmation(candidate);

				return Redirect("/Candidates"); 
			}

			return RedirectToAction(nameof(Nominate));
		}

		public IActionResult Thanks()
		{
			return View();
		}

	}
}
