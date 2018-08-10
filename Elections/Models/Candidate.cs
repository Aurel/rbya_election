using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elections.Models
{
	public class Candidate : BaseModel
	{
		[Required]
		[Display(Name = "Candidate Name", GroupName = "Candidate")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Home Church Name", GroupName = "Candidate"), DefaultValue("")]
		public string Church { get; set; }

		[Required]
		[Display(Name = "City, State", GroupName = "Candidate")]
		public string Location { get; set; }

		[Display(Name = "Email", GroupName = "Candidate"), DefaultValue("")]
		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[Display(Name = "Position", GroupName = "Candidate")]
		[DefaultValue(Position.Committee)]
		public Position Position { get; set; } = Position.Committee;

		[Required, MinLength(10), MaxLength(5000), DefaultValue("")]
		[Display(Name = "Candidate Background", GroupName = "Candidate")]
		public string Background { get; set; }

		[Required, MinLength(10), MaxLength(5000), DefaultValue("")]
		[Display(Name = "Reasons for Nomination", GroupName = "Candidate")]
		public string Reasons { get; set; }

		[Required]
		[Display(Name = "Submitter Name", GroupName = "Submitter")]
		[DefaultValue("")]
		public string Submitter { get; set; }

		[Display(Name = "Submitter Email", GroupName = "Submitter")]
		[EmailAddress]
		[Required]
		[DefaultValue("elections@rbya.org")]
		public string SubmitterEmail { get; set; }

		public DateTime CreatedDate { get; set; }

		public Guid Guid { get; set; }

		public bool Accepted { get; set; }
		public bool Approved { get; set; }
		public bool Seconded { get; set; }
		public bool Archived { get; set; }

		public string ImageUrl { get; set; }
		public bool Selected { get; set; }
	}
}
