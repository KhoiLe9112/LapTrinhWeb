using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
	public class PaginationSearchProduct : PaginationSearchBaseResult
	{
		public IList<Product>? Data { get; set; }
	}
}
