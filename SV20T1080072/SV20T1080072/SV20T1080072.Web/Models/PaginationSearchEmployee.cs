using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
	public class PaginationSearchEmployee : PaginationSearchBaseResult
	{
		public IList<Employee> Data { get; set; }
	}
}
