using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
	public class PaginationSearchShipper : PaginationSearchBaseResult
	{
		public IList<Shipper> Data { get; set; }
	}
}
