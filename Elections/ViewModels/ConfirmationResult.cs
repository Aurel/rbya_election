using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elections.ViewModels
{
    public class ConfirmationResult
    {
		public Guid UniqueId { get; set; }
		public bool Accepted { get; set; }
		public string Signature { get; set; }
	}
}
