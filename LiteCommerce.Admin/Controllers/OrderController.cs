using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.STAFF_SALE)]
    public class OrderController : Controller
    {
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;

            List<Order> listOfOrders = OrderBLL.ListOfOrders(page, pageSize, searchValue, out rowCount);
            var model = new Models.OrderPaginationResult()
            {
                Data = listOfOrders,
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue
            };
            return View(model);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}