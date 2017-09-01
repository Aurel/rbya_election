using System.ComponentModel.DataAnnotations;

namespace Elections.Models
{
	public class BaseModel
	{
		[Key]
		public int Id { get; set; }
	}
}
