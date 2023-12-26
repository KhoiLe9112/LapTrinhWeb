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

		public static List<Order> ListOrders(string searchValue = "")
		{
			return orderDB.List(1, 0, searchValue, 0, 0, 1).ToList();
		}

		public static List<Order> ListOrders(int page, int pageSize, string searchValue, int customerID, int shipperID, int status, out int rowCount)
		{
			rowCount = orderDB.Count(searchValue, status);
			return orderDB.List(page, pageSize, searchValue, customerID, shipperID, status).ToList();
		}

		public static Order? GetOrder(int orderID)
		{
			return orderDB.Get(orderID);
		}

		public static int AddOrder(Order data, IEnumerable<OrderDetail> details)
		{
			return orderDB.Add(data, details);
		}

		public static bool UpdateOrder(Order data)
		{
			return orderDB.Update(data);
		}

		public static bool DeleteOrder(int orderID)
		{
			return orderDB.Delete(orderID);
		}
	}
}
