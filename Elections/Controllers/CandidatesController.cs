using Elections.Models;
using Elections.Services;
using Elections.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elections.Controllers
{
	public class CandidatesController : Controller
	{
		private readonly ElectionContext _context;
		private readonly Mailer _mailer;
		public CandidatesController(ElectionContext context, Mailer mailer)
		{
			_context = context;
			_mailer = mailer;
		}
        
		public async Task<IActionResult> SuperSecretAdminPage()
		{
			return View("Index", await _context.Candidates.ToListAsync());
		}



		public IActionResult Index()
		{
			var model = _context.Candidates
			.Where(x => !x.Ignored)
			.GroupBy(x => x.Position).Select(x => new PositionalGrouping
			{
				Candidates = x.OrderBy(c => c.Name).ToList(),
				MaxCandidates = x.Key == Position.Committee ? 15 : 1,
				Position = x.Key
			});

			return View("Candidates", model);
		}



        [HttpGet, Route("/comment/{id}")]
        public async Task<IActionResult> Comment(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .SingleOrDefaultAsync(m => m.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View("Comment", new Comment { Candidate = candidate });
        }


        [HttpPost, Route("/comment/{id}")]
        public async Task<IActionResult> CommentSubmission(Comment comment, [FromRoute(Name = "id")] int? id)
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

            if (comment == null)
            {
                return BadRequest("Problem with submitting comment. Please message us to get this resolved ASAP.");
            }
            comment.Id = 0;
            comment.Candidate = candidate;

            _context.Add(comment);
            await _context.SaveChangesAsync();

            if (candidate.State == CandidateState.Accepted && comment.Type == Models.Comment.CommentType.Positive)
            {
                candidate.Ready = true;
            }

            await _context.SaveChangesAsync();


            return Json(comment);
        }

        #region Confirmation
        public async Task<IActionResult> SendConfirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .SingleOrDefaultAsync(m => m.Id == id && !m.Ignored);

            if (candidate == null)
            {
                return NotFound("Could not find a matching candidate with the given ID");
            }

            _mailer.SendCandidateConfirmation(candidate);

            return Ok();
        }


        [Route("confirmation/{guid}")]
		public async Task<IActionResult> Confirmation(Guid? guid)
		{
			if (guid == null)
			{
				return NotFound();
			}

			var candidate = await _context.Candidates
				.SingleOrDefaultAsync(m => m.Guid == guid);

			if(candidate == null)
			{
				return NotFound("We could not find the candidate you were looking for. If you were trying to access your confirmation page and it did not work, please message us on Facebook.");
			}

			return View("Confirmation", candidate);
		}

		[HttpPost]
		[Route("confirmation/confirm")]
		public async Task<IActionResult> Confirm(ConfirmationResult confirmation)
		{
			
			if (confirmation == null || confirmation.UniqueId == null)
			{
				return NotFound();
			}

			var candidate = await _context.Candidates
				.SingleOrDefaultAsync(m => m.Guid == confirmation.UniqueId);

			if (candidate == null)
			{
				return NotFound("We could not find the candidate you were looking for. If you were trying to submit your confirmation page and it did not work, please message us on Facebook.");
			}

			candidate.Confirmed = true;
			candidate.Accepted = confirmation.Accepted;

			_context.Update(candidate);
			await _context.SaveChangesAsync();

			var result = "Thank you for submitting your response.";
			if(confirmation.Accepted)
			{
				result += "Your response has been noted and you are now eligible to be seconded.";
			}
			else
			{
				result += "Your response has been noted and you will be removed from the candidates list.";
			}

			return Ok(result);
		}

        #endregion

        #region Create

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
		public async Task<IActionResult> Create([Bind("Name,Church,Location,Position,Reasons,Background,Submitter,Comments,ImageUrl,Selected,Id")] Candidate candidate)
		{
			if (ModelState.IsValid)
			{
				_context.Add(candidate);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(candidate);
		}

		#endregion

		#region Edit
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
		public async Task<IActionResult> Edit(int id, Candidate candidate)
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


		#endregion

		#region Delete
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

        #endregion


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



        private bool CandidateExists(int id)
		{
			return _context.Candidates.Any(e => e.Id == id);
		}
	}
}
