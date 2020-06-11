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
    public class CountryDAL : ICountryDAL
    {
        private string connectionString;
        public CountryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Country> List()
        {
            List<Country> data = new List<Country>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // tạo câu lệnh query
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Countries order by CountryName";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Country()
                        {
                            CountryID = Convert.ToInt32(dbReader["CountryID"]),
                            CountryName = Convert.ToString(dbReader["CountryName"])
                        });
                    }
                }

                connection.Close();
            }
            return data;
        }
    }
}
