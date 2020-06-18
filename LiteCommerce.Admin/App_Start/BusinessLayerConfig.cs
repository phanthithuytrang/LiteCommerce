using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class BusinessLayerConfig
    {
        public static void Init()
        {
            string connectionString = 
                ConfigurationManager.ConnectionStrings["LiteCommerce"].ConnectionString;
            //TODO: Khởi tạo các BLL khi càn sử dụng đến
            CatalogBLL.Initialize(connectionString);
            EmployeeBLL.Initialize(connectionString);
            UserAccountBLL.Initialize(connectionString);
            OrderBLL.Initialize(connectionString);
        }
    }
}