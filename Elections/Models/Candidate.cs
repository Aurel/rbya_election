namespace Elections.Models
{
	public class Candidate : BaseModel
	{
		public string Name { get; set; }
		public string Reasons { get; set; }
		public string Background { get; set; }
		public string Church { get; set; }
		public string Location { get; set; }
	}
}
