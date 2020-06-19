using LiteCommerce.Admin.Codes;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.STAFF_ACCOUNT)]
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Employee> listOfEmployee = EmployeeBLL.ListOfEmployees(page, pageSize, searchValue, out rowCount);

            var model = new Models.EmployeePaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = listOfEmployee

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
                    ViewBag.Title = "Create new Employee";
                    Employee newEmployee = new Employee()
                    {
                        EmployeeID = 0
                    };
                    return View(newEmployee);
                }
                else
                {
                    ViewBag.Title = "Edit a Employee";
                    Employee editEmployee = EmployeeBLL.GetEmployee(Convert.ToInt32(id));
                    if (editEmployee == null)
                        return RedirectToAction("Index");
                    return View(editEmployee);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ":" + ex.StackTrace);
            }
        }
        [HttpPost]
        public ActionResult Input(Employee model, HttpPostedFileBase uploadFile)
        {
            try
            {
                //TODO: Kiểm tra tính hợp le của dự liệu được nhập
                if (string.IsNullOrEmpty(model.LastName))
                    ModelState.AddModelError("LastName", "LastName expected");
                if (string.IsNullOrEmpty(model.FirstName))
                    ModelState.AddModelError("FirstName", "FirstName expected");
                if (string.IsNullOrEmpty(model.Title))
                    ModelState.AddModelError("Title", "Title expected");
                if (string.IsNullOrEmpty(model.Email))
                    ModelState.AddModelError("Email", "Email expected");
                if (string.IsNullOrEmpty(model.Address))
                    ModelState.AddModelError("Address", "Address expected");
                if (string.IsNullOrEmpty(model.City))
                    model.City = "";
                if (string.IsNullOrEmpty(model.Country))
                    model.Country = "";
                if (string.IsNullOrEmpty(model.HomePhone))
                    model.HomePhone = "";
                if (string.IsNullOrEmpty(model.Notes))
                    model.Notes = "";
                if (string.IsNullOrEmpty(model.PhotoPath))
                    model.PhotoPath = "";
                if (string.IsNullOrEmpty(model.Password))
                {
                    model.Password = Encode.EncodeMD5("123456");
                }

                if (uploadFile != null)
                {
                    string folder = Server.MapPath("../Images/Uploads");
                    //string fileName = uploadfile.FileName;
                    string fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(uploadFile.FileName)}";
                    string filePath = Path.Combine(folder, fileName);
                    uploadFile.SaveAs(filePath);
                    model.PhotoPath = fileName;
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.EmployeeID == 0 ? "Add new employee" : "Edit a employee";
                    return View(model);
                }
                //TODO: Lưu dữ liệu vào DB
                if (model.EmployeeID == 0)
                {
                    EmployeeBLL.AddEmployee(model);
                }
                else
                {
                    EmployeeBLL.UpdateEmployee(model);
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
        /// <param name="EmployeeIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] employeeIDs = null)
        {
            if (employeeIDs != null)
                EmployeeBLL.DeleteEmployees(employeeIDs);
            return RedirectToAction("Index");
        }
     
    }
}