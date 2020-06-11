using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Authorize]
    public class DashboardController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }
    }
}