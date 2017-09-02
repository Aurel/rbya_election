namespace Elections.Models
{
	public class Vote : BaseModel
	{
		public Candidate Candidate { get; set; }
		public Voter Voter { get; set; }
		public bool For { get; set; }
	}
}
