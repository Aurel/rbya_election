using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elections.Models
{
	public class Election : BaseModel
	{
		[Required]
		public int Year { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Election Day")]
		public DateTime ElectionDay { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Nomination Cutoff Day")]
		public DateTime NominationCutoff { get; set; }

		[Display(Name = "Is voting currently open?")]
		public bool VotingOpen { get; set; }
		
		[Display(Name = "Are nominations open?")]
		public bool NominationsOpen { get; set; }
	}
}
