using LiteCommerce.DataLayer;
using LiteCommerce.DataLayer.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class OrderBLL
    {
        public static void Initialize(string connectionString)
        {
            OrderDB = new OrderDAL(connectionString);
        }

        #region Khai báo các thuộc tính giao tiếp với DAL
        private static IOrderDAL OrderDB { get; set; }
        #endregion

        public static List<Order> ListOfOrders(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 3;
            rowCount = OrderDB.Count(searchValue);
            return OrderDB.List(page, pageSize, searchValue);
        }
    }
}
