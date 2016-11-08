using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.WebUI.Controllers
{
    public class HomeController : BasicController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}