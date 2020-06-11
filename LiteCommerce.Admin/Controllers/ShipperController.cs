using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class ShipperController : Controller
    {
        [Authorize]
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Shipper> listOfShipper = CatalogBLL.ListOfShippers(page, pageSize, searchValue, out rowCount);

            var model = new Models.ShipperPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = listOfShipper

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
                    ViewBag.Title = "Create new Shipper";
                    Shipper newShipper = new Shipper()
                    {
                        ShipperID = 0
                    };
                    return View(newShipper);
                }
                else
                {
                    ViewBag.Title = "Edit a Shipper";
                    Shipper editShipper = CatalogBLL.GetShipper(Convert.ToInt32(id));
                    if (editShipper == null)
                        return RedirectToAction("Index");
                    return View(editShipper);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ":" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult Input(Shipper model)
        {
            try
            {
                //TODO: Kiểm tra tính hợp lệ của dự liệu được nhập
                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "CompanyName expected");
                if (string.IsNullOrEmpty(model.Phone))
                    model.Phone = "";

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.ShipperID == 0 ? "Add new Shipper" : "Edit a Shipper";
                    return View(model);
                }
                //TODO: Lưu dữ liệu vào DB
                if (model.ShipperID == 0)
                {
                    CatalogBLL.AddShipper(model);
                }
                else
                {
                    CatalogBLL.UpdateShipper(model);
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
        /// <param name="ShipperIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] ShipperIDs = null)
        {
            if (ShipperIDs != null)
                CatalogBLL.DeleteShippers(ShipperIDs);
            return RedirectToAction("Index");
        }
    }
}