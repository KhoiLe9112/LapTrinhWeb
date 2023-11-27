using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
	public class PaginationSearchCustomer : PaginationSearchBaseResult
	{
		public IList<Customer> Data { get; set; }
	}
}
