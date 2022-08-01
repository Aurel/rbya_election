using Elections.Models;
using System;
using System.Linq;

namespace Elections.Services
{
	public class ElectionDecider
	{
		private ElectionContext _context;

		public ElectionDecider(ElectionContext context)
		{
			_context = context;
		}

		public int Year => GetCurrentElection().Year;

		public Election GetCurrentElection()
		{
			var now = DateTime.UtcNow;
			var a = _context.Elections.FirstOrDefault(election => election.Year == now.Year);

			if (a == null)
			{
				a = _context.Elections.OrderByDescending(x => x.Year).FirstOrDefault(x => x.Year <= now.Year);

				if (a == null)
				{
					throw new InvalidOperationException($"Tried to get an election for year {now.Year} but could not find one in the system. Please add one to proceed.");
				}
			}

			return a;
		}
	}
}
