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
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;
        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Order data)
        {
            int orderID = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Orders
                                          (CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipperID, ShipAddress, ShipCity, ShipCountry)
                                          VALUES (
                                                @CustomerID, 
                                                @EmployeeID, 
                                                @OrderDate, 
                                                @RequiredDate,
                                                @ShippedDate, 
                                                @ShipperID, 
                                                @ShipAddress,
                                                @ShipCity, 
                                                @ShipCountry
                                                );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@OrderDate", data.OrderDate);
                cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);

                orderID = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return orderID;
        }

        public int Count(string customerID, int employeeID, int shipperID)
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Orders as O join Customers as C on O.CustomerID = C.CustomerID
                                     where (C.CustomerID = @CustomerID OR @CustomerID = '') 
                                   AND (EmployeeID = @EmployeeID OR @EmployeeID = 0) 
                                   AND (ShipperID = @ShipperID OR @ShipperID = 0)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@ShipperID", shipperID);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }

        public int Delete(int[] OrderIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Orders
                                            WHERE(OrderID = @OrderID)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@OrderID", SqlDbType.Int);
                foreach (int orderID in OrderIDs)
                {
                    cmd.Parameters["@OrderID"].Value = orderID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Order Get(int orderID)
        {
            Order data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Orders WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Order()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            OrderDate = Convert.IsDBNull(dbReader["OrderDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["OrderDate"]),
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            RequiredDate = Convert.IsDBNull(dbReader["RequiredDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            ShippedDate = Convert.IsDBNull(dbReader["ShippedDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["ShippedDate"]),
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Order> List(int page, int pageSize, string customerID, int employeeID, int shipperID)
        {
            List<Order> data = new List<Order>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from (
	                                      select ROW_NUMBER() over(order by C.CompanyName) as RowNumber,
                                          O.OrderID, O.CustomerID, C.CompanyName as CustomerCompanyName, O.EmployeeID, (E.FirstName + ' ' + E.LastName) as EmployeeName,
                                          O.OrderDate, O.RequiredDate, O.ShippedDate, O.ShipperID, S.CompanyName as ShipperCompanyName,
                                          O.ShipAddress, O.ShipCity, O.ShipCountry
	                                    from Orders as O join Customers as C on O.CustomerID = C.CustomerID
                                            join Employees as E on O.EmployeeID = E.EmployeeID
                                            join Shippers as S on O.ShipperID = S.ShipperID
                                            where (C.CustomerID = @CustomerID OR @CustomerID = '') 
                                            AND (E.EmployeeID = @EmployeeID OR @EmployeeID = 0) 
                                            AND (S.ShipperID = @ShipperID OR @ShipperID = 0)
                                        ) as t
                                        where (@pageSize <= 0) or
                                            (t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@ShipperID", shipperID);

                using (SqlDataReader dbReader = cmd.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        Order order = new Order()
                        {
                            CustomerCompanyName = Convert.ToString(dbReader["CustomerCompanyName"]),
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            EmployeeName = Convert.ToString(dbReader["EmployeeName"]),
                            OrderDate = Convert.IsDBNull(dbReader["OrderDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["OrderDate"]),
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            RequiredDate = Convert.IsDBNull(dbReader["RequiredDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            ShippedDate = Convert.IsDBNull(dbReader["ShippedDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["ShippedDate"]),
                            ShipperCompanyName = Convert.ToString(dbReader["ShipperCompanyName"]),
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"])
                        };
                        data.Add(order);
                    }
                }

                connection.Close();
            }
            return data;
        }

        public bool Update(Order data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Orders
                                           SET [CustomerID] = @CustomerID 
                                              ,[EmployeeID] = @EmployeeID
                                              ,[OrderDate] = @OrderDate
                                              ,[RequiredDate] = @RequiredDate
                                              ,[ShippedDate] = @ShippedDate
                                              ,[ShipperID] = @ShipperID
                                              ,[ShipAddress] = @ShipAddress
                                              ,[ShipCity] = @ShipCity
                                              ,[ShipCountry] = @ShipCountry
                                          WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@OrderDate", data.OrderDate);
                cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);
                cmd.Parameters.AddWithValue("@OrderID", data.OrderID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
