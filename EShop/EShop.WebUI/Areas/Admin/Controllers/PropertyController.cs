using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.WebUI.Areas.Admin.Controllers
{
    public class PropertyController : BasicController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}