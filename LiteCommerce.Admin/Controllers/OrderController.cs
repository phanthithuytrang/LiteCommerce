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
        public ActionResult Index(int page = 1, string customerID = "", int employeeID = 0, int shipperID = 0)
        {
            int pageSize = 3;
            int rowCount = 0;

            List<Order> listOfOrders = OrderBLL.ListOfOrders(page, pageSize, customerID, employeeID, shipperID, out rowCount);
            var model = new Models.OrderPaginationResult()
            {
                Data = listOfOrders,
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                CustomerID = customerID,
                EmployeeID = employeeID,
                SearchValue = "",
                ShipperID = shipperID
            };
            return View(model);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = " Create new Order";
                Order order = new Order()
                {
                    OrderID = 0
                };
                return View(order);
            }
            else
            {
                ViewBag.Title = "Edit a Order";
                Order order = OrderBLL.Get(Convert.ToInt32(id));
                return View(order);
            }
        }

        [HttpPost]
        public ActionResult Create(Order order)
        {
            //Kiểm tra hợp lệ dữ liệu
            if (order.OrderDate == null)
            {
                order.OrderDate = new DateTime(2000, 1, 1);
            }
            if (string.IsNullOrEmpty(order.CustomerID))
            {
                ModelState.AddModelError("CustomerID", "Please select a customer");
            }
            if (order.EmployeeID == 0)
            {
                ModelState.AddModelError("EmployeeID", "Please select an Employee");
            }
            if (order.RequiredDate == null)
            {
                order.OrderDate = new DateTime(2000, 1, 1);
            }
            if (string.IsNullOrEmpty(order.ShipAddress))
            {
                ModelState.AddModelError("ShipAddress", "Please enter ship address");
            }
            if (string.IsNullOrEmpty(order.ShipCity))
            {
                ModelState.AddModelError("ShipCity", "Please enter ship city");
            }
            if (string.IsNullOrEmpty(order.ShipCountry))
            {
                ModelState.AddModelError("ShipCountry", "Please enter ship country");
            }
            if (order.ShippedDate == null)
            {
                order.ShippedDate = new DateTime(2000, 1, 1);
            }
            if (order.ShipperID == 0)
            {
                ModelState.AddModelError("ShipperID", "Please select Shipper");
            }

            if (ModelState.IsValid)
            {
                //Lưu vào DB
                if (order.OrderID == 0)
                {
                    //Tạo mới
                    OrderBLL.AddOrder(order);
                }
                else
                {
                    //Sửa
                    OrderBLL.Update(order);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(order);
            }
        }

        public ActionResult Delete(int[] orderIDs)
        {
            if (orderIDs.Length > 0)
            {
                OrderBLL.DeleteOrder(orderIDs);
            }
            return RedirectToAction("Index");
        }
    }
}