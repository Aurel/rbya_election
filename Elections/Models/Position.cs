using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elections.Models
{
	public enum Position
	{
		[Display(Name = "President")]
		President,
		[Display(Name = "Vice President - East Coast")]
		VicePresidentEast,
		[Display(Name = "Vice President - West Coast")]
		VicePresidentWest,
		[Display(Name = "Treasurer")]
		Treasurer,
		[Display(Name = "Controller")]
		Controller,
		[Display(Name = "Committee Member")]
		Committee
	}
}
