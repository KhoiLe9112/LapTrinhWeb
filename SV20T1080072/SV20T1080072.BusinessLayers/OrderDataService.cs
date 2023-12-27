using SV20T1080072.DataLayers;
using SV20T1080072.DataLayers.SQLServer;
using SV20T1080072.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080072.BusinessLayers
{
    public class OrderDataService
    {
        private static readonly IOderDAL orderDB;

        static OrderDataService()
        {
            string connectionString = "server=.;user id=sa;password=lmkhoi123;database=LiteCommerceDB;TrustServerCertificate=true";
            orderDB = new OrderDAL(connectionString);
        }

        public static List<Order> ListOrders(int page, int pageSize, int status, string searchValue, out int rowCount)
        {
            rowCount = orderDB.Count(searchValue, status);
            return orderDB.List(page, pageSize, searchValue, status).ToList();
        }

        public static Order GetOrder(int orderID)
        {
            return orderDB.Get(orderID);
        }

        /// <summary>
        /// Khởi tạo đơn hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <param name="orderTime"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static int InitOrder(int customerID, int employeeID, DateTime orderTime, IEnumerable<OrderDetail> details)
        {
            Order data = new Order()
            {
                CustomerID = customerID,
                EmployeeID = employeeID,
                OrderTime = orderTime,
                AcceptTime = null,
                ShipperID = null,
                ShippedTime = null,
                FinishedTime = null,
                Status = OrderStatus.INIT
            };
            return orderDB.Add(data, details);
        }

        /// <summary>
        /// Hủy đơn
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool CancelOrder(int orderID)
        {
            Order data = orderDB.Get(orderID);
            if (data == null)
                return false;

            //TODO: Kiểm tra xem việc hủy bỏ đơn hàng có hợp lý đối với trạng thái hiện tại của đơn hàng hay không?
            //... Your code here ...

            data.Status = OrderStatus.CANCEL;
            data.FinishedTime = DateTime.Now;
            return orderDB.Update(data);
        }

        /// <summary>
        /// Từ chối đơn
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool RejectOrder(int orderID)
        {
            Order data = orderDB.Get(orderID);
            if (data == null)
                return false;

            //TODO: Kiểm tra xem việc từ chối đơn hàng có hợp lý đối với trạng thái hiện tại của đơn hàng hay không?
            //... Your code here ...

            data.Status = OrderStatus.REJECTED;
            data.FinishedTime = DateTime.Now;
            return orderDB.Update(data);
        }

        /// <summary>
        /// Chấp nhận đơn
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool AcceptOrder(int orderID)
        {
            Order data = orderDB.Get(orderID);
            if (data == null)
                return false;

            //TODO: Kiểm tra xem việc chấp nhận đơn hàng có hợp lý đối với trạng thái hiện tại của đơn hàng hay không?
            //... Your code here ...

            data.Status = OrderStatus.ACCEPTED;
            data.AcceptTime = DateTime.Now;
            return orderDB.Update(data);
        }

        /// <summary>
        /// Xác nhận giao hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static bool ShipOrder(int orderID, int shipperID)
        {
            Order data = orderDB.Get(orderID);
            if (data == null)
                return false;

            //TODO: Kiểm tra xem việc xác nhận đã chuyển hàng có hợp lý đối với trạng thái hiện tại của đơn hàng hay không?
            //... Your code here ...

            data.Status = OrderStatus.SHIPPING;
            data.ShipperID = shipperID;
            data.ShippedTime = DateTime.Now;
            return orderDB.Update(data);
        }

        /// <summary>
        /// Xác nhận xử lý đơn hàng thành công
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static bool FinishOrder(int orderID)
        {
            Order data = orderDB.Get(orderID);
            if (data == null)
                return false;

            //TODO: Kiểm tra xem việc ghi nhận đơn hàng kết thúc thành công có hợp lý đối với trạng thái hiện tại của đơn hàng hay không?
            //... Your code here ...

            data.Status = OrderStatus.FINISHED;
            data.FinishedTime = DateTime.Now;
            return orderDB.Update(data);
        }

        public static bool DeleteOrder(int orderID)
        {
            var data = orderDB.Get(orderID);
            if (data == null)
                return false;

            if (data.Status == OrderStatus.INIT || data.Status == OrderStatus.CANCEL || data.Status == OrderStatus.REJECTED)
                return orderDB.Delete(orderID);

            return false;
        }

        public static List<OrderDetail> ListOrderDetails(int orderID)
        {
            return orderDB.ListDetails(orderID).ToList();
        }

        public static OrderDetail GetOrderDetail(int orderID, int productID)
        {
            return orderDB.GetDetail(orderID, productID);
        }

        public static int SaveOrderDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            return orderDB.SaveDetail(orderID, productID, quantity, salePrice);
        }

        public static bool DeleteOrderDetail(int orderID, int productID)
        {
            return orderDB.DeleteDetail(orderID, productID);
        }

        //      public static Order? GetOrder(int orderID)
        //{
        //	return orderDB.Get(orderID);
        //}

        //public static int AddOrder(Order data, IEnumerable<OrderDetail> details)
        //{
        //	return orderDB.Add(data, details);
        //}

        //public static bool UpdateOrder(Order data)
        //{
        //	return orderDB.Update(data);
        //}
    }
}
