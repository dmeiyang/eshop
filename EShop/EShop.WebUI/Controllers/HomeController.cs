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
        public void Index()
        {
            Response.Write(string.Format("我是系统首页！！！亲爱的{0}，欢迎回来", User.Identity.Name));
            Response.Write(string.Format("我是系统首页！！！亲爱的{0}，欢迎回来", CurrentUser));
        }
    }
}