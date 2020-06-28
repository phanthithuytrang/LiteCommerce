using LiteCommerce.DataLayer;
using LiteCommerce.DataLayer.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class UserAccountBLL
    {
        private static string _connectionString;
        private static IUserAccountDAL userAccountDB { get; set; }
        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static UserAccount Authorize(string userName, string password, UserAccountTypes userType)
        {
            switch (userType)
            {
                case UserAccountTypes.Employee:
                    userAccountDB = new EmployeeUserAccountDAL(_connectionString);
                    break;
                case UserAccountTypes.Customer:
                    userAccountDB = new CustomerUserAccountDAL(_connectionString);
                    break;
                default:
                    return null;
            }
            return userAccountDB.Authorize(userName, password);
        }
        public static bool ChangePassword(string password, string email)
        {
            return userAccountDB.ChangePassword(password, email);
        }
    }
}
