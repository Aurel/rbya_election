﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Elections.Models;

namespace Elections.Controllers
{
	public class VotersController : Controller
	{
		private readonly ElectionContext _context;

		public VotersController(ElectionContext context)
		{
			_context = context;
		}

		// GET: Voters
		public async Task<IActionResult> Index()
		{
			return View(await _context.Voters.ToListAsync());
		}

		// GET: Voters/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var voter = await _context.Voters
				.SingleOrDefaultAsync(m => m.Id == id);
			if (voter == null)
			{
				return NotFound();
			}

			return View(voter);
		}

		// GET: Voters/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Voters/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Email,Code,Id")] Voter voter)
		{
			if (ModelState.IsValid)
			{
				_context.Add(voter);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(voter);
			//System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes("f36145e13e32ab59ac0304cf3d2f1994"));


		}

		// GET: Voters/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var voter = await _context.Voters.SingleOrDefaultAsync(m => m.Id == id);
			if (voter == null)
			{
				return NotFound();
			}
			return View(voter);
		}

		// POST: Voters/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Name,Email,Code,Id")] Voter voter)
		{
			if (id != voter.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(voter);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!VoterExists(voter.Id))
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
			return View(voter);
		}

		// GET: Voters/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var voter = await _context.Voters
				.SingleOrDefaultAsync(m => m.Id == id);
			if (voter == null)
			{
				return NotFound();
			}

			return View(voter);
		}

		// POST: Voters/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var voter = await _context.Voters.SingleOrDefaultAsync(m => m.Id == id);
			_context.Voters.Remove(voter);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool VoterExists(int id)
		{
			return _context.Voters.Any(e => e.Id == id);
		}
	}
}
