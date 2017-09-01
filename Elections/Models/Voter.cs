namespace Elections.Models
{
	public class Voter : BaseModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Code { get; set; }
	}
}