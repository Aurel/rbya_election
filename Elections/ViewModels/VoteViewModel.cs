using Elections.Models;
using System.Collections.Generic;

namespace Elections
{
	public class PositionalGrouping
	{
		public Position Position { get; set; }
		public List<Candidate> Candidates { get; set; }
		public int MaxCandidates { get; set; }
	}

	public class StatePositionalGrouping
	{
		public List<PositionalGrouping> Groupings;
		public CandidateState state;
	}
}
