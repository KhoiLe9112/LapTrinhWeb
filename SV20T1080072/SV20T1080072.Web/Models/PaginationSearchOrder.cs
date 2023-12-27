using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
    public class PaginationSearchOrder : PaginationSearchBaseResult
    {
        public IList<Order> Data { get; set; }
    }
}
