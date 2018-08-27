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

		public enum CommentType
		{
			Positive, 
			Negative
		}
	}
}
