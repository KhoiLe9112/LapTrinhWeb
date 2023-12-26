using Dapper;
using SV20T1080072.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080072.DataLayers.SQLServer
{
	public class OrderDAL : _BaseDAL, IOderDAL
	{
		public OrderDAL(string connectionString) : base(connectionString)
		{
		}

        /// <summary>
        /// Chuyển dữ liệu từ SqlDataReader thành Order
        /// </summary>
        /// <param name="dbReader"></param>
        /// <returns></returns>
        //private Order DataRowToOrder(DataRow dataRow)
        //{
        //    return new Order()
        //    {
        //        OrderID = Convert.ToInt32(dataRow["OrderID"]),
        //        OrderTime = Convert.ToDateTime(dataRow["OrderTime"]),
        //        AcceptTime = DBValueToNullableDateTime(dataRow["AcceptTime"]),
        //        ShippedTime = DBValueToNullableDateTime(dataRow["ShippedTime"]),
        //        FinishedTime = DBValueToNullableDateTime(dataRow["FinishedTime"]),
        //        Status = Convert.ToInt32(dataRow["Status"]),
        //        CustomerID = DBValueToNullableInt(dataRow["CustomerID"]),
        //        CustomerName = dataRow["CustomerName"].ToString(),
        //        CustomerContactName = dataRow["CustomerContactName"].ToString(),
        //        CustomerAddress = dataRow["CustomerAddress"].ToString(),
        //        CustomerEmail = dataRow["CustomerEmail"].ToString(),

        //        EmployeeID = DBValueToNullableInt(dataRow["EmployeeID"]),
        //        EmployeeFullName = $"{dataRow["EmployeeFirstName"]} {dataRow["EmployeeLastName"]}",

        //        ShipperID = DBValueToNullableInt(dataRow["ShipperID"]),
        //        ShipperName = dataRow["ShipperName"].ToString(),
        //        ShipperPhone = dataRow["ShipperPhone"].ToString()
        //    };
        //}

        public int Add(Order data)
		{
			int id = 0;
			using (var connection = OpenConnection())
			{
				var sql = @"
                            insert into Orders(CustomerID,OrderTime,DeliveryProvince,DeliveryAddress,EmployeeID,AcceptTime,ShipperID,ShippedTime,FinishedTime,Status)
                            values(@CustomerID,@OrderTime,@DeliveryProvince,@DeliveryAddress,@EmployeeID,@AcceptTime,@ShipperID,@ShippedTime,@FinishedTime,@Status);
                            select @@identity;
						";
				var parameters = new
				{
					customerId = data.CustomerID,
					orderTime = data.OrderTime,
					deliveryProvince = data.DeliveryProvince ?? "",
					deliveryAddress = data.DeliveryAddress ?? "",
					employeeId = data.EmployeeID,
					acceptTime = data.AcceptTime,
					shipperId = data.ShipperID,
					shippedTime = data.ShippedTime,
					finishedTime = data.FinishedTime,
					status = data.Status
				};
				id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
				connection.Close();
			}
			return id;
		}

        public int Add(Order data, IEnumerable<OrderDetail> details)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "", int status = 1)
		{
			int count = 0;
			if (!string.IsNullOrEmpty(searchValue))
				searchValue = "%" + searchValue + "%";
			using (var connection = OpenConnection())
			{
				var sql = @"select count(*) from Orders
                            where (@searchValue = N'') or (CustomerName like @searchValue) or (ShipperName like @searchValue)";
				var parameter = new
				{
					searchValue,
				};
				count = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: CommandType.Text);
				connection.Close();
			}
			return count;
		}

		public bool Delete(int orderID)
		{
			throw new NotImplementedException();
		}

        public bool DeleteDetail(int orderID, int productID)
        {
            throw new NotImplementedException();
        }

        public Order? Get(int orderID)
		{
			Order? data = null;
			using (var connection = OpenConnection())
			{
				var sql = "select * from Orders where OrderID = @orderId";
				var parameters = new { orderId = orderID };
				data = connection.QueryFirstOrDefault<Order>(sql: sql, param: parameters, commandType: CommandType.Text);
				connection.Close();
			}
			return data;
		}

        public OrderDetail GetDetail(int orderID, int productID)
        {
            throw new NotImplementedException();
        }

        public IList<Order> List(int page = 1, int pageSize = 0, string searchValue = "", int customerID = 0, int shipperID = 0, int status = 1)
		{
			List<Order> data = new List<Order>();
			if (!string.IsNullOrEmpty(searchValue))
				searchValue = "%" + searchValue + "%";
			using (var connection = OpenConnection())
			{
				var sql = @"with cte as
                            (
	                            select *, ROW_NUMBER() over (order by CustomerName) as RowNumber
	                            from Customers
	                            where (@searchValue = N'' or (CustomerName like @searchValue))
                            )
                            select * from cte
                            where (@pageSize = 0)
	                            or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";

				var parameters = new
				{
					page,   //nếu trùng tên
					pageSize = pageSize,
					searchValue = searchValue
				};
				data = (connection.Query<Order>(sql: sql, param: parameters, commandType: CommandType.Text)).ToList();
				connection.Close();
			}
			if (data == null)
				data = new List<Order>();
			return data;
		}

        public IList<OrderDetail> ListDetails(int orderID)
        {
            throw new NotImplementedException();
        }

        public int SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order data)
		{
			throw new NotImplementedException();
		}
	}
}
