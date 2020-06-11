using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class CategoryController : Controller
    {
        [Authorize]
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Category> listOfCategory = CatalogBLL.ListOfCategory(page, pageSize, searchValue, out rowCount);

            var model = new Models.CategoryPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = listOfCategory

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
                    ViewBag.Title = "Create new Category";
                    Category newCategory = new Category()
                    {
                        CategoryID = 0
                    };
                    return View(newCategory);
                }
                else
                {
                    ViewBag.Title = "Edit a Category";
                    Category editCategory = CatalogBLL.GetCategory(Convert.ToInt32(id));
                    if (editCategory == null)
                        return RedirectToAction("Index");
                    return View(editCategory);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ":" + ex.StackTrace);
            }
        }
        [HttpPost]
        public ActionResult Input(Category model)
        {
            try
            {
                //TODO: Kiểm tra tính hợp lleej của dự liệu được nhập
                if (string.IsNullOrEmpty(model.CategoryName))
                    ModelState.AddModelError("CategoryName", "CategoryName expected");
                if (string.IsNullOrEmpty(model.Description))
                    model.Description = "";

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.CategoryID == 0 ? "Add new Category" : "Edit a Category";
                    return View(model);
                }
                //TODO: Lưu dữ liệu vào DB
                if (model.CategoryID == 0)
                {
                    CatalogBLL.AddCategory(model);
                }
                else
                {
                    CatalogBLL.UpdateCategory(model);
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
        public ActionResult Delete(int[] CategoryIDs = null)
        {
            if (CategoryIDs != null)
                CatalogBLL.DeleteCategories(CategoryIDs);
            return RedirectToAction("Index");
        }
    }
}