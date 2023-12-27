using SV20T1080072.DomainModels;

namespace SV20T1080072.Web.Models
{
    public class PaginationSearchOrderDetail
    {
        public Order Order { get; set; }
        public List<DomainModels.OrderDetail> OrderDetails { get; set; }
    }
}
