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
	public class ProductDAL : _BaseDAL, IProductDAL
	{
		public ProductDAL(string connectionString) : base(connectionString)
		{
		}

		public int Add(Product data)
		{
			int id = 0;
			using (var connection = OpenConnection())
			{
				var sql = @"if exists(select * from Products where ProductName = @ProductName)
                                select -1
                            else
                                begin
                                    insert into Products(ProductName,ProductDescription,SupplierID,CategoryID,Unit,Price,Photo,IsSelling)
                                    values(@ProductName,@ProductDescription,@SupplierId,@CategoryId,@Unit,@Price,@Photo,@IsSelling);
                                    select @@identity;
                                end";
				var parameters = new
				{
					productName = data.ProductName ?? "",
					productDescription = data.ProductDescription ?? "",
					supplierId = data.SupplierId,
					categoryId = data.CategoryId,
					unit = data.Unit ?? "",
					price = data.Price,
					photo = data.Photo ?? "",
					IsSelling = data.IsSelling
				};
				id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
				connection.Close();
			}
			return id;
		}

		public long AddAttribute(ProductAttribute data)
		{
			throw new NotImplementedException();
		}

		public long AddPhoto(ProductPhoto data)
		{
			throw new NotImplementedException();
		}

		public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
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

		public bool Delete(int productID)
		{
			bool result = false;
			using (var connection = OpenConnection())
			{
				var sql = "delete from Products where productID = @productId and not exists(select * from OrderDetails where ProductID = @productId)";
				var parameters = new { productId = productID };
				result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
				connection.Close();
			}
			return result;
		}

		public bool DeleteAttribute(long attributeID)
		{
			throw new NotImplementedException();
		}

		public bool DeletePhoto(long photoID)
		{
			throw new NotImplementedException();
		}

		public Product? Get(int productID)
		{
			Product? data = null;
			using (var connection = OpenConnection())
			{
				var sql = "select * from Products where ProductID = @productId";
				var parameters = new { customerId = productID };
				data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
				connection.Close();
			}
			return data;
		}

		public ProductAttribute? GetAttribute(long attributeID)
		{
			throw new NotImplementedException();
		}

		public ProductPhoto? GetPhoto(long photoID)
		{
			throw new NotImplementedException();
		}

		public bool InUsed(int productID)
		{
			throw new NotImplementedException();
		}

		public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
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

		public IList<ProductAttribute> ListAttributes(int productID)
		{
			throw new NotImplementedException();
		}

		public IList<ProductPhoto> ListPhotos(int productID)
		{
			throw new NotImplementedException();
		}

		public bool Update(Product data)
		{
			bool result = false;
			using (var connection = OpenConnection())
			{
				var sql = @"if not exists(select * from Products where ProductID <> @productId and ProductName = @productName)
                                begin
                                    update Products 
                                    set ProductName = @productName,
                                        ProductDescription = @productDescription,
                                        SupplierID = @supplierId,
                                        CategoryID = @categoryId,
                                        Unit = @unit,
                                        Price = @price,
                                        Photo = @photo,
										IsSelling = @IsSelling
                                    where CustomerID = @customerId
                                end";
				var parameters = new
				{
					productName = data.ProductName ?? "",
					productDescription = data.ProductDescription ?? "",
					supplierId = data.SupplierId,
					categoryId = data.CategoryId,
					unit = data.Unit ?? "",
					price = data.Price,
					photo = data.Photo ?? "",
					IsSelling = data.IsSelling
				};
				result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
			}
			return result;
		}

		public bool UpdateAttribute(ProductAttribute data)
		{
			throw new NotImplementedException();
		}

		public bool UpdatePhoto(ProductPhoto data)
		{
			throw new NotImplementedException();
		}
	}
}
