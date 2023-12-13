using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
	public class PaginationSearchCategory : PaginationSearchBaseResult
	{
		public IList<Category> Data { get; set; }
	}
}
