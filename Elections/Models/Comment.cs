using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elections.Models
{
	public class Comment : BaseModel
	{
		[Required, MinLength(10), MaxLength(5000), DefaultValue("")]
		[Display(Name = "Comments about the Candidate")]
		public string Content { get; set; }

		[DefaultValue(CommentType.Positive)]
		public CommentType Type { get; set; }

		public Candidate Candidate { get; set; }

        [Required]
        [Display(Name = "Submitter Name", GroupName = "Submitter")]
        [DefaultValue("")]
        public string Submitter { get; set; }

        [Display(Name = "Submitter Email", GroupName = "Submitter")]
        [EmailAddress]
        [Required]
        [DefaultValue("elections@rbya.org")]
        public string SubmitterEmail { get; set; }

        [ReadOnly(true)]
        public DateTime CreatedDate { get; set; }

        public enum CommentType
		{
			Positive,
			Negative
		}
	}
}
