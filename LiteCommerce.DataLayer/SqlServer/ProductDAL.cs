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
    public class ProductDAL : IProductDAL
    {
        private string connectionString;

        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Product data)
        {
            int productId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products
                                          (
	                                          ProductName,
	                                          SupplierID,
	                                          CategoryID,
	                                          QuantityPerUnit,
	                                          UnitPrice,
	                                          Descriptions,
	                                          PhotoPath
                                          )
                                          VALUES
                                          (
	                                          @productName,
	                                          @supplierID,
	                                          @categoryID,
	                                          @quantityPerUnit,
	                                          @unitPrice,
	                                          @descriptions,
	                                          @photoPath
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("productName", data.ProductName);
                cmd.Parameters.AddWithValue("supplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("categoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("quantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("unitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("photoPath", data.PhotoPath);

                productId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return productId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue, string category, string supplier)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) from Products 
                                    where ((@searchValue = N'') or (ProductName like @searchValue))
                                            and ((@category = 0) or (CategoryID = @category))
                                            and ((@supplier = 0) or (SupplierID = @supplier))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@supplier", supplier);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return count;
        }

        public bool Delete(int[] productIDs)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products
                                            WHERE(ProductID = @productId)";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@productId", SqlDbType.Int);
                foreach (int productId in productIDs)
                {
                    cmd.Parameters["@productId"].Value = productId;
                    rowsAffected += Convert.ToInt32(cmd.ExecuteNonQuery());
                }

                connection.Close();
            }
            return rowsAffected == productIDs.Length;
        }

        public Product Get(int productID)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID = @productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@productID", productID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Product()
                        {
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Product> List(int page, int pageSize, string searchValue, string category, string supplier)
        {
            List<Product> data = new List<Product>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                          from
                                          (
	                                            select	row_number() over(order by ProductName) as RowNumber,
			                                          Products.*
	                                            from	Products
	                                            where	((@searchValue = N'') or (ProductName like @searchValue))
                                                    and ((@category = 0) or (CategoryID = @category))
                                                    and ((@supplier = 0) or (SupplierID = @supplier))
                                          ) as t
                                          where 
                                               (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                          order by t.RowNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@supplier", supplier);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Product()
                        {
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"])
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                           SET ProductName = @ProductName
                                              , SupplierID = @SupplierID
                                              , CategoryID = @CategoryID
                                              , QuantityPerUnit = @QuantityPerUnit
                                              , UnitPrice = @UnitPrice
                                              , Descriptions = @Descriptions
                                              , PhotoPath = @PhotoPath
                                          WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("PhotoPath", data.PhotoPath);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
