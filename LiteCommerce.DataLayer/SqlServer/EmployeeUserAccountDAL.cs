using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer.SqlServer
{
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public UserAccount Authorize(string userName, string password)
        {
            UserAccount data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT EmployeeID, LastName, FirstName, Title, PhotoPath, Roles
                                    FROM Employees
                                    WHERE Email = @email AND Password = @password";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@email", userName);
                cmd.Parameters.AddWithValue("@password", password);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new UserAccount()
                        {
                            UserID = dbReader["EmployeeID"].ToString(),
                            FullName = dbReader["FirstName"].ToString(),
                            Photo = dbReader["PhotoPath"].ToString(),
                            Title = dbReader["Title"].ToString(),
                            Roles = dbReader["Roles"].ToString()
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool ChangePassword(string Password, string email)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                           SET Password = @Password
                                          WHERE Email = @email";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@email", email);
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }   
    }
}
