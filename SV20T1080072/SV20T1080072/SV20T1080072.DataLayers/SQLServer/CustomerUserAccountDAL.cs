using Dapper;
using SV20T1080072.DomainModels;
using System.Data;

namespace SV20T1080072.DataLayers.SQLServer
{
    public class CustomerUserAccountDAL : _BaseDAL, IUserAccountDAL
    {
        public CustomerUserAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string userName, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select CustomerID as UserID, Email as UserName, CustomerName as FullName, Email 
                            from Customers 
                            where Email = @userName and Password = @password";
                var parameters = new
                {
                    userName = userName,
                    password = password
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool ChangePassword(string userName, string password)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Customers set Password = @password where Email = @userName";
                var parameter = new
                {
                    userName = userName,
                    password = password
                };
                result = connection.Execute(sql: sql, param: parameter, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
