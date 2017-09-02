using Elections.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Elections.Controllers
{
	public class CandidatesController : Controller
	{
		private readonly ElectionContext _context;

		public CandidatesController(ElectionContext context)
		{
			_context = context;
		}

		// GET: Candidates

		public async Task<IActionResult> Candidate()
		{
			return View("Index", await _context.Candidates.ToListAsync());
		}

		public async Task<IActionResult> Index()
		{
			var model = _context.Candidates.GroupBy(x => x.Position).Select(x => new PositionalGrouping
			{
				Candidates = x.OrderBy(c => c.Name).ToList(),
				MaxCandidates = x.Key == Position.Committee ? 15 : 1,
				Position = x.Key
			});

			return View("Candidates", model);	
		}

		// GET: Candidates/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var candidate = await _context.Candidates
				.SingleOrDefaultAsync(m => m.Id == id);
			if (candidate == null)
			{
				return NotFound();
			}

			return View(candidate);
		}

		// GET: Candidates/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Candidates/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Church,Location,Position,Reasons,Background,Submitter,Comments,ImageUrl,Id")] Candidate candidate)
		{
			if (ModelState.IsValid)
			{
				_context.Add(candidate);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(candidate);
		}

		// GET: Candidates/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var candidate = await _context.Candidates.SingleOrDefaultAsync(m => m.Id == id);
			if (candidate == null)
			{
				return NotFound();
			}
			return View(candidate);
		}

		// POST: Candidates/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Name,Church,Location,Position,Reasons,Background,Submitter,Comments,ImageUrl,Id")] Candidate candidate)
		{
			if (id != candidate.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(candidate);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CandidateExists(candidate.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(candidate);
		}

		// GET: Candidates/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var candidate = await _context.Candidates
				.SingleOrDefaultAsync(m => m.Id == id);
			if (candidate == null)
			{
				return NotFound();
			}

			return View(candidate);
		}

		// POST: Candidates/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var candidate = await _context.Candidates.SingleOrDefaultAsync(m => m.Id == id);
			_context.Candidates.Remove(candidate);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CandidateExists(int id)
		{
			return _context.Candidates.Any(e => e.Id == id);
		}
	}
}
