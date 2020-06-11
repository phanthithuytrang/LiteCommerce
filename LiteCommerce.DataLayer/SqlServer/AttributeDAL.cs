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
    public class AttributeDAL : IAttributeDAL
    {
        private string connectionString;
        public AttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Attributes> List()
        {
            List<Attributes> data = new List<Attributes>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // tạo câu lệnh query
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Attributes";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Attributes()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            AttributeName = Convert.ToString(dbReader["AttributeName"])
                        });
                    }
                }

                connection.Close();
            }
            return data;
        }
    }
}
