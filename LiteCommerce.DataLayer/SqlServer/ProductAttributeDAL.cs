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
    public class ProductAttributeDAL : IProductAttributeDAL
    {
        private string connectionString;
        public ProductAttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(ProductAttribute data)
        {
            int attributeID = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO ProductAttributes
                                          (
	                                          ProductID,
	                                          AttributeName,
	                                          AttributeValues,
	                                          DisplayOrder
                                          )
                                          VALUES
                                          (
	                                          @ProductID,
	                                          @AttributeName,
	                                          @AttributeValues,
	                                          @DisplayOrder
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = data.ProductID;
                cmd.Parameters.Add("@AttributeName", SqlDbType.NVarChar).Value = data.AttributeName;
                cmd.Parameters.Add("@AttributeValues", SqlDbType.NVarChar).Value = data.AttributeValues;
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int).Value = data.DisplayOrder;
                attributeID = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return attributeID;
        }

        public int Delete(int[] attributeIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM ProductAttributes
                                            WHERE(AttributeID = @attributeID)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@attributeID", SqlDbType.Int);
                foreach (int attributeID in attributeIDs)
                {
                    cmd.Parameters["@attributeID"].Value = attributeID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }
        public ProductAttribute Get(int attributeID)
        {
            ProductAttribute data = new ProductAttribute();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductAttributes WHERE AttributeID = @attributeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@attributeID", attributeID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new ProductAttribute()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            AttributeName = Convert.ToString(dbReader["AttributeName"]),
                            AttributeValues = Convert.ToString(dbReader["AttributeValues"]),
                            DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<ProductAttribute> List(int productID)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *from ProductAttributes where ProductID = @productID order by DisplayOrder";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@productID", productID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new ProductAttribute()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            AttributeName = Convert.ToString(dbReader["AttributeName"]),
                            AttributeValues = Convert.ToString(dbReader["AttributeValues"]),
                            DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"])
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(ProductAttribute data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductAttributes
                                           SET AttributeName = @AttributeName 
                                              ,AttributeValues = @AttributeValues
                                              ,DisplayOrder = @DisplayOrder
                                          WHERE AttributeID = @AttributeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@AttributeID", data.AttributeID);
                cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@AttributeValues", data.AttributeValues);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
