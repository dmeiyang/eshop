using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.WebUI.Controllers
{
    [Authorize(Roles ="user")]
    public class BasicController : Controller
    {
        protected string CurrentUser { get { return User.Identity.GetUserId(); } }

        public void Test()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            foreach (var v in identity.Claims)
            {
                Response.Write(string.Format("类型：{0}；值：{1}", v.Type, v.Value));
                Response.Write("<hr/>");
            }
        }
    }
}