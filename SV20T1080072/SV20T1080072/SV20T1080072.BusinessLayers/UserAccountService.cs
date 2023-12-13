using SV20T1080072.DataLayers;
using SV20T1080072.DomainModels;

namespace SV20T1080072.BusinessLayers
{
    public class UserAccountService
    {
        private static readonly IUserAccountDAL employeeUserAccountDB;
        private static readonly IUserAccountDAL customerUserAccountDB;

        static UserAccountService()
        {
            string connectionString = "server=.;user id=sa;password=lmkhoi123;database=LiteCommerceDB;TrustServerCertificate=true";
            employeeUserAccountDB = new DataLayers.SQLServer.EmployeeUserAccountDAL(connectionString);
            customerUserAccountDB = new DataLayers.SQLServer.CustomerUserAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(string userName, string password, TypeOfAccounts typeOfAccounts)
        {
            switch (typeOfAccounts)
            {
                case TypeOfAccounts.Employee:
                    return employeeUserAccountDB.Authorize(userName, password);
                case TypeOfAccounts.Customer:
                    return customerUserAccountDB.Authorize(userName, password);
                default:
                    return null;
            }
        }

        public static bool ChangePassword(string userName, string password, TypeOfAccounts typeOfAccounts)
        {
            switch (typeOfAccounts)
            {
                case TypeOfAccounts.Employee:
                    return employeeUserAccountDB.ChangePassword(userName, password);
                case TypeOfAccounts.Customer:
                    return customerUserAccountDB.ChangePassword(userName, password);
                default:
                    return false;
            }
        }

        public enum TypeOfAccounts
        {
            Employee,
            Customer,
            Shipper
        }
    }
}
