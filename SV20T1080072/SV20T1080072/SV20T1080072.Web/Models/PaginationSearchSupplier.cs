using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
	public class PaginationSearchSupplier : PaginationSearchBaseResult
	{
		public IList<Supplier> Data { get; set; }
	}
}
