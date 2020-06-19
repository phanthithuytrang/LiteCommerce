using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.STAFF_DATA)]
    public class CountryController : Controller
    {
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Country> listOfCountry = CatalogBLL.ListOfCountries(page, pageSize, searchValue, out rowCount);

            var model = new Models.CountryPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = listOfCountry

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
                    ViewBag.Title = "Create new Country";
                    Country newCountry = new Country()
                    {
                        CountryID = 0
                    };
                    return View(newCountry);
                }
                else
                {
                    ViewBag.Title = "Edit a Country";
                    Country editCountry = CatalogBLL.GetCountry(Convert.ToInt32(id));
                    if (editCountry == null)
                        return RedirectToAction("Index");
                    return View(editCountry);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ":" + ex.StackTrace);
            }
        }
        [HttpPost]
        public ActionResult Input(Country model)
        {
            try
            {
                //TODO: Kiểm tra tính hợp lleej của dự liệu được nhập
                if (string.IsNullOrEmpty(model.CountryName))
                    ModelState.AddModelError("CountryName", "CountryName expected");
               
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.CountryID == 0 ? "Add new Country" : "Edit a Country";
                    return View(model);
                }
                //TODO: Lưu dữ liệu vào DB
                if (model.CountryID == 0)
                {
                    CatalogBLL.AddCountry(model);
                }
                else
                {
                    CatalogBLL.UpdateCountry(model);
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
        /// <param name="CategoryIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] CountryIDs = null)
        {
            if (CountryIDs != null)
                CatalogBLL.DeleteCountries(CountryIDs);
            return RedirectToAction("Index");
        }
    }
}
