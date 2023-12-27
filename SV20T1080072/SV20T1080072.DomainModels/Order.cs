using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080072.DomainModels
{
    public class Order
    {
        /// <summary>
        /// Thông tin đơn hàng
        /// </summary>
        public int OrderID { get; set; }      
        public DateTime OrderTime { get; set; }
        public string DeliveryProvince { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public DateTime? AcceptTime { get; set; }
        public DateTime? ShippedTime { get; set; }
        public DateTime? FinishedTime { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// Thông tin khách hàng của đơn hàng
        /// </summary>
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContactName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Thông tin nhân viên phụ trách đơn hàng
        /// </summary>
        public int? EmployeeID { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;

        /// <summary>
        /// Thông tin người giao hàng phụ trách đơn hàng
        /// </summary>
        public int? ShipperID { get; set; }
        public string ShipperName { get; set; } = string.Empty;
        public string ShipperPhone { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả trạng thái đơn hàng dựa trên mã trạng thái
        /// </summary>
        public string StatusDescription
        {
            get
            {
                switch (Status)
                {
                    case OrderStatus.INIT:
                        return "Đơn hàng mới. Đang chờ duyệt";
                    case OrderStatus.ACCEPTED:
                        return "Đơn đã chấp nhận. Đang chờ chuyển hàng";
                    case OrderStatus.SHIPPING:
                        return "Đơn hàng đang được giao";
                    case OrderStatus.FINISHED:
                        return "Đơn hàng đã hoàn tất";
                    case OrderStatus.CANCEL:
                        return "Đơn hàng đã bị hủy";
                    case OrderStatus.REJECTED:
                        return "Đơn hàng bị từ chối";
                    default:
                        return "";
                }
            }
        }
    }

    /// <summary>
    /// Thông tin chi tiết mặt hàng bán trong đơn hàng
    /// </summary>
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal SalePrice { get; set; } = 0;
        /// <summary>
        /// Thành tiền = Số lượng * Giá bán
        /// </summary>
        public decimal TotalPrice => Quantity * SalePrice;
    }

    /// <summary>
    /// Định nghĩa trạng thái đơn hàng
    /// </summary>
    public class OrderStatus
    {
        public const int INIT = 1;
        public const int ACCEPTED = 2;
        public const int SHIPPING = 3;
        public const int FINISHED = 4;
        public const int CANCEL = -1;
        public const int REJECTED = -2;
    }
}
