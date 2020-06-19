using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.STAFF_DATA)]
    public class ProductController : Controller
    {
        [Authorize]
        public ActionResult Index(string category = "", string supplier = "", string searchValue = "", int page = 1)
        {
            int pageSize = 10;
            int rowCount = 0;
            //Muốn sử dụng out bắt buộc phải chỉ định giá trị trước khi sử dụng
            List<Product> listOfProduct = CatalogBLL.ListOfProducts(page, pageSize, searchValue, out rowCount, category, supplier);
            var model = new Models.ProductPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Category = category,
                Supplier = supplier,
                Data = listOfProduct
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
                    ViewBag.Title = "Create new product";
                    ViewBag.Method = "Add";
                    Product newProduct = new Product()
                    {
                        ProductID = 0
                    };
                    Models.ProductAttributeResult model = new Models.ProductAttributeResult()
                    {
                        Product = newProduct,
                        ProductAttributes = null
                    };
                    return View(model);
                }
                else
                {
                    ViewBag.Title = "Edit a product";
                    ViewBag.Method = "Edit";
                    Product editProduct = CatalogBLL.GetProduct(Convert.ToInt32(id));
                    List<ProductAttribute> attr = CatalogBLL.ListOfProductAttribute(Convert.ToInt32(id));
                    Models.ProductAttributeResult model = new Models.ProductAttributeResult()
                    {
                        Product = editProduct,
                        ProductAttributes = attr
                    };
                    if (editProduct == null)
                        return RedirectToAction("Index");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ": " + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult Input(Product model, HttpPostedFileBase uploadFile)
        {
            Models.ProductAttributeResult models = new Models.ProductAttributeResult();
            try
            {
                if (string.IsNullOrEmpty(model.ProductName))
                {
                    ModelState.AddModelError("ProductName", "ProductName expected");
                }
                if ((model.SupplierID == 0))
                {
                    ModelState.AddModelError("SupplierID", "SupplierID expected");
                }
                if (model.CategoryID == 0)
                {
                    ModelState.AddModelError("CategoryID", "CategoryID expected");
                }
                if (string.IsNullOrEmpty(model.QuantityPerUnit))
                {
                    ModelState.AddModelError("QuantityPerUnit", "QuantityPerUnit expected");
                }
                if (model.UnitPrice <= 0)
                {
                    ModelState.AddModelError("UnitPrice", "UnitPrice expected");
                }
                if (string.IsNullOrEmpty(model.Descriptions))
                {
                    ModelState.AddModelError("Descriptions", "Descriptions expected");
                }
                if (string.IsNullOrEmpty(model.PhotoPath))
                    model.PhotoPath = "";

                if (uploadFile != null)
                {
                    string folder = Server.MapPath("../Images/Uploads");
                    //string fileName = uploadfile.FileName;
                    string fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(uploadFile.FileName)}";
                    string filePath = Path.Combine(folder, fileName);
                    uploadFile.SaveAs(filePath);
                    model.PhotoPath = fileName;
                }

                if (ViewBag.Method == "Add")
                {
                    ViewBag.Title = "Create new product";
                    ViewBag.Method = "Add";
                    Product newProduct = new Product()
                    {
                        ProductID = 0
                    };
                    models = new Models.ProductAttributeResult()
                    {
                        Product = newProduct,
                        ProductAttributes = null
                    };
                }
                else if (ViewBag.Method == "Edit")
                {
                    ViewBag.Title = "Edit a product";
                    ViewBag.Method = "Edit";
                    Product editProduct = CatalogBLL.GetProduct(Convert.ToInt32(model.ProductID));
                    List<ProductAttribute> attr = CatalogBLL.ListOfProductAttribute(Convert.ToInt32(model.ProductID));
                    models = new Models.ProductAttributeResult()
                    {
                        Product = editProduct,
                        ProductAttributes = attr
                    };
                }

                if (!ModelState.IsValid)
                {
                    return View(models);
                }

                if (model.ProductID == 0)
                {
                    CatalogBLL.AddProduct(model);
                }
                else
                {
                    CatalogBLL.UpdateProduct(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Attribute(string ProductID = "", string AttributeID = "")
        {
            try
            {
                if (string.IsNullOrEmpty(AttributeID))
                {
                    ViewBag.Title = "Create new product attributes";
                    ProductAttribute newProductAtribute = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = Convert.ToInt32(ProductID)
                    };
                    return View(newProductAtribute);
                }
                else
                {
                    ViewBag.Title = "Edit product attributes";
                    ProductAttribute editProductAttribute = CatalogBLL.GetProductAttribute(Convert.ToInt32(AttributeID));
                    if (editProductAttribute == null)
                        return RedirectToAction("Input/" + Convert.ToString(ProductID));
                    return View(editProductAttribute);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ":" + ex.StackTrace);
            }
        }
        [HttpPost]
        public ActionResult Attribute(ProductAttribute model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AttributeValues))
                    ModelState.AddModelError("AttributeValues", "AttributeValues expected!");
                if (string.IsNullOrEmpty(model.AttributeName))
                    ModelState.AddModelError("AttributeName", "AttributeName expected!");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //TODO: Lưu dữ liệu vào DB
                if (model.AttributeID == 0)
                {
                    CatalogBLL.AddProductAttribute(model);
                }
                else
                {
                    CatalogBLL.UpdateProductAttribute(model);
                }
                return RedirectToAction("Input/" + Convert.ToString(model.ProductID));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Delete(int[] productIDs)
        {
            if (productIDs != null)
            {
                CatalogBLL.DeleteProduct(productIDs);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteAttribute(int productID, int[] attributeIDs = null)
        {
            if (attributeIDs != null)
                CatalogBLL.DeleteProductAttributes(attributeIDs);
            return RedirectToAction("Input/" + Convert.ToString(productID));
        }
    }
}