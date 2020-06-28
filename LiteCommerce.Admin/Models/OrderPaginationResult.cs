using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class OrderPaginationResult : PaginationResult
    {
        public string CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public int ShipperID { get; set; }
        public List<Order> Data { get; set; }
    }
}