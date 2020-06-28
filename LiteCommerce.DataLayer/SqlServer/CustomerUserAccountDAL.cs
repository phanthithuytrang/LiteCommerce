using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer.SqlServer
{
    public class CustomerUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        public CustomerUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public UserAccount Authorize(string userName, string Password)
        {
            return new UserAccount()
            {
                UserID = userName,
                FullName = "Nguyễn A",
                Photo = "60d93c00a10496df2994d97d907f59c4.jpg",
                Title = "Vice President, Sales"
            };
        }
        public bool ChangePassword(string Password, string email)
        {
            throw new NotImplementedException();
        }
    }
}
