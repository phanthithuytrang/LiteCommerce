using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class CustomerController : Controller
    {
        [Authorize]
        public ActionResult Index(int page = 1, string searchValue = "")
        {

            int pageSize = 3;
            int rowCount = 0;
            List<Customer> listOfCustomer = CatalogBLL.ListOfCustomer(page, pageSize, searchValue, out rowCount);

            var model = new Models.CustomerPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = listOfCustomer

            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Input(string id = "")
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new Customer";
                    Customer newCustomer = new Customer()
                    {
                        CustomerID = ""
                    };
                    return View(newCustomer);
                }
                else
                {
                    ViewBag.Title = "Edit a Customer";
                    Customer editCustomer = CatalogBLL.GetCustomer(id);
                    if (editCustomer == null)
                        return RedirectToAction("Index");
                    return View(editCustomer);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ":" + ex.StackTrace);
            }
        }
        [HttpPost]
        public ActionResult Input(Customer model)
        {
            try
            {
                //TODO: Kiểm tra tính hợp lleej của dự liệu được nhập
                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "CompanyName expected");
                if (string.IsNullOrEmpty(model.ContactName))
                    ModelState.AddModelError("ContactName", "ContactName expected");
                if (string.IsNullOrEmpty(model.ContactTitle))
                    ModelState.AddModelError("ContactTitle", "ContactTitle expected");
                if (string.IsNullOrEmpty(model.Address))
                    ModelState.AddModelError("Address", "Address expected");
                if (string.IsNullOrEmpty(model.City))
                    model.City = "";
                if (string.IsNullOrEmpty(model.Country))
                    model.Country = "";
                if (string.IsNullOrEmpty(model.Phone))
                    model.Phone = "";
                if (string.IsNullOrEmpty(model.Fax))
                    model.Fax = "";

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.CustomerID == "" ? "Add new Customer" : "Edit a Customer";
                    return View(model);
                }
                //TODO: Lưu dữ liệu vào DB
                Customer existCustomer = CatalogBLL.GetCustomer(model.CustomerID);
                if (existCustomer == null)
                {
                    CatalogBLL.AddCustomer(model);
                }
                else
                {
                    CatalogBLL.UpdateCustomer(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string[] CustomerIDs = null)
        {
            if (CustomerIDs != null)
                CatalogBLL.DeleteCustomers(CustomerIDs);
            return RedirectToAction("Index");
        }
    }
}