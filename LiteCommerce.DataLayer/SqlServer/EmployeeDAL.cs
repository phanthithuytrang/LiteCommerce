using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer.SqlServer
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private string connectionString;
        public EmployeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Employee data)
        {
            int EmployeeId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees
                                          (
	                                          LastName,
	                                          FirstName,
	                                          Title,
                                              BirthDate,
                                              HireDate,
                                              Email,
	                                          Address,
	                                          City,
	                                          Country,
	                                          HomePhone,
	                                          Notes,
	                                          PhotoPath,
                                              Password
                                          )
                                          VALUES
                                          (
	                                          @LastName,
	                                          @FirstName,
	                                          @Title,
                                              @BirthDate,
                                              @HireDate,
                                              @Email,
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @HomePhone,
	                                          @Notes,
	                                          @PhotoPath,
                                              @Password
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);

                EmployeeId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return EmployeeId;
        }

        public int CheckEmail(string email)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Employees
                                    where Email = @email";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@email", email);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return count;
        }

        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from Employees where @searchValue = N'' or LastName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }

        public int Delete(int[] EmployeeIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Employees
                                            WHERE(EmployeeID = @EmployeeId)
                                            AND(EmployeeID NOT IN(SELECT EmployeeID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
                foreach (int EmployeeId in EmployeeIDs)
                {
                    cmd.Parameters["@EmployeeId"].Value = EmployeeId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Employee Get(int EmployeeID)
        {
            Employee data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Password = Convert.ToString(dbReader["Password"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Employee> List(int page, int pageSize, string searchValue)
        {
            List<Employee> data = new List<Employee>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                    (
	                                    select ROW_NUMBER() over(Order by LastName) as RowNumber,
	                                    Employees.*
	                                    from Employees
	                                    where (@searchValue = N'') or (LastName like @searchValue)
                                    ) as t
                                    where t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize
                                    order by t.RowNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Password = Convert.ToString(dbReader["Password"])

                        });
                    }
                }

                connection.Close();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                           SET LastName = @LastName 
                                              ,FirstName = @FirstName
                                              ,Title = @Title
                                              ,BirthDate = @BirthDate
                                              ,HireDate = @HireDate
                                              ,Email = @Email
                                              ,Address = @Address
                                              ,City = @City
                                              ,Country = @Country
                                              ,HomePhone = @HomePhone
                                              ,Notes = @Notes
                                              ,PhotoPath = @PhotoPath
                                          WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            return rowsAffected > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ChangePassword(int userID, string password)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                           SET Password = @Password
                                          WHERE EmployeeID = @UserID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@UserID", userID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }


            return rowsAffected > 0;
        }
    }
}
