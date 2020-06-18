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
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Orders as O join Customers as C on O.CustomerID = C.CustomerID
                                    where @searchValue = N'' or C.CompanyName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }

        public int Delete(int[] OrderIDs)
        {
            throw new NotImplementedException();
        }

        public Order Get(int OrderID)
        {
            throw new NotImplementedException();
        }

        public List<Order> List(int page, int pageSize, string searchValue)
        {
            List<Order> data = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from (
	                                    select ROW_NUMBER() over(order by OrderID) as RowNumber,
                                        O.OrderID, O.CustomerID, C.CompanyName as CustomerCompanyName, O.EmployeeID, (E.FirstName + ' ' + E.LastName) as EmployeeName,
                                        O.OrderDate, O.RequiredDate, O.ShippedDate, O.ShipperID, S.CompanyName as ShipperCompanyName,
                                        O.ShipAddress, O.ShipCity, O.ShipCountry
	                                    from Orders as O join Customers as C on O.CustomerID = C.CustomerID
                                            join Employees as E on O.EmployeeID = E.EmployeeID
                                            join Shippers as S on O.ShipperID = S.ShipperID
	                                    where (@searchValue = N'') or (C.CompanyName like @searchValue)
                                    ) as t
                                    where (@pageSize <= 0) or
                                        (t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

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
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
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
            throw new NotImplementedException();
        }
    }
}
