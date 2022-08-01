using Elections.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Elections.Controllers
{
	public class ElectionsController : Controller
	{
		private readonly ElectionContext _context;

		public ElectionsController(ElectionContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var candidates = _context.Elections;

			
			return View(candidates);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Models.Election election)
		{
			if(_context.Elections.Any(e => e.Year == election.Year))
			{
				return BadRequest("An election already exists with that given year.");
			}

			if (ModelState.IsValid)
			{
				_context.Add(election);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

	}
}
