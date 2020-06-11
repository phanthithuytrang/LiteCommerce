using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
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
            if(email=="phanthithuytranggg@gmail.com" && password=="123456")
            {
                //Ghi nhận phiên đăng nhập của tài khoản 
                System.Web.Security.FormsAuthentication.SetAuthCookie(email, false);
                //Chuyển trang về Dashboard
                return RedirectToAction("Index", "Dashboard");
            }   
            else
            {
                ModelState.AddModelError("LoginError", "Login Fail");
                ViewBag.Email = email;
                return View();
            }    
        }

        [AllowAnonymous]
        public ActionResult FogotPassword()
        {
            return View();
        }
    }
}