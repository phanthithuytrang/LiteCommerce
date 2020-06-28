using LiteCommerce.Admin.Codes;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LiteCommerce.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            WebUserData userData = User.GetUserData();
            Employee employee = EmployeeBLL.GetEmployee(Convert.ToInt32(userData.UserID));
            return View(employee);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string userID, string oldPassword, string newPassword, string confirmPassword)
        {
            //kiểm tra hợp lệ dữ liệu
            if (string.IsNullOrEmpty(userID))
            {
                ModelState.AddModelError("userID", "UserID is invalid");
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                ModelState.AddModelError("oldPassword", "Old Password is invalid");
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("newPassword", "New Password is invalid");
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("confirmPassword", "Confirm Password is invalid");
            }
            if (!newPassword.Equals(confirmPassword))
            {
                ModelState.AddModelError("notMatch", "New Password and Confirm Password must match");
            }
            Employee existEmployee = EmployeeBLL.GetEmployee(Convert.ToInt32(userID));
            oldPassword = Encode.EncodeMD5(oldPassword);
            newPassword = Encode.EncodeMD5(newPassword);
            if (!existEmployee.Password.Equals(oldPassword))
            {
                ModelState.AddModelError("wrongPassword", "Password is wrong");
            }

            if (ModelState.IsValid)
            {
                //Lưu thay đổi
                if (EmployeeBLL.ChangePassword(Convert.ToInt32(userID), newPassword))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string email="", string password="" )
        {
            //TODO: Kiểm tra tài khoản thông qua CSDL
            password = Encode.EncodeMD5(password);
            UserAccount user = UserAccountBLL.Authorize(email, 
                                                        password,
                                                        UserAccountTypes.Employee);
            if (user != null)
            {
                //Ghi nhận phiên đăng nhập
                WebUserData userData = new WebUserData()
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    GroupName = user.Roles,//TODO: cần thay đổi cho đúng
                    LoginTime = DateTime.Now,
                    SessionID = Session.SessionID,
                    ClientIP = Request.UserHostAddress,
                    Photo = user.Photo,
                    Title = user.Title
                };
                FormsAuthentication.SetAuthCookie(userData.ToCookieString(), false);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("LoginError", "Login Fail");
                ViewBag.Email = email;
                return View();
            }
            //if(email=="phanthithuytranggg@gmail.com" && password=="123456")
            //{
            //    //Ghi nhận phiên đăng nhập của tài khoản 
            //    System.Web.Security.FormsAuthentication.SetAuthCookie(email, false);
            //    //Chuyển trang về Dashboard
            //    return RedirectToAction("Index", "Dashboard");
            //}   
            //else
            //{
            //    ModelState.AddModelError("LoginError", "Login Fail");
            //    ViewBag.Email = email;
            //    return View();
            //}    
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string email = "", string pwn = "", string confirmpw = "")
        {
            ViewBag.Email = email;
            if (pwn.Equals(confirmpw))
            {
                if (EmployeeBLL.CheckEmail(email) != 0)
                {
                    UserAccountBLL.ChangePassword(Encode.EncodeMD5(pwn), email);
                    return RedirectToAction("SignIn", "Account");
                }
                else
                {
                    ModelState.AddModelError("Messege", "Email không tồn tại!");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Messege", "Mật Khẩu không khớp!");
                return View();
            }

        }

    }
} 
