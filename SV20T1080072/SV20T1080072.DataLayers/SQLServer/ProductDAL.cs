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
	public class ProductDAL : _BaseDAL, ICommonDAL<Product>
	{
		public ProductDAL(string connectionString) : base(connectionString)
		{
		}

		public int Add(Product data)
		{
			throw new NotImplementedException();
		}

		public int Count(string searchValue = "")
		{
			int count = 0;
			if (!string.IsNullOrEmpty(searchValue))
				searchValue = "%" + searchValue + "%";
			using (var connection = OpenConnection())
			{
				var sql = @"select count(*) from Products
                            where (@searchValue = N'') or (ProductName like @searchValue)";
				var parameter = new
				{
					searchValue,
				};
				count = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: CommandType.Text);
				connection.Close();
			}

			return count;
		}

		public bool Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Product? Get(int id)
		{
			throw new NotImplementedException();
		}

		public bool InUsed(int id)
		{
			throw new NotImplementedException();
		}

		public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "")
		{
			List<Product> data = new List<Product>();
			if (!string.IsNullOrEmpty(searchValue))
				searchValue = "%" + searchValue + "%";
			using (var connection = OpenConnection())
			{
				var sql = @"with cte as
                            (
	                            select *, ROW_NUMBER() over (order by ProductName) as RowNumber
	                            from Products
	                            where (@searchValue = N'' or (ProductName like @searchValue))
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
				data = (connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text)).ToList();
				connection.Close();
			}
			if (data == null)
				data = new List<Product>();
			return data;
		}

		public bool Update(Product data)
		{
			throw new NotImplementedException();
		}
	}
}
