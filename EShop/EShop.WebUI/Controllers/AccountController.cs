using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace EShop.WebUI.Controllers
{
    public class AccountController : Controller
    {
        protected IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        protected ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }

        protected ApplicationRoleManager RoleManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); } }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string account, string password)
        {
            // 1. 利用ASP.NET Identity获取用户对象
            var user = await UserManager.FindAsync(account, password);

            if (user == null)
                return Json(new { Flag = false, Content = "用户名或密码错误！！！" });

            // 2. 利用ASP.NET Identity获取identity 对象
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // 3. 将上面拿到的identity对象登录
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

            return Json(new { Flag = true, Content = "登录成功！！！" });
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(Models.RegisterViewModel model)
        {
            var user = new ApplicationUser { Id = IDHelper.Id32, UserName = model.UserName, Email = model.Email, Type = 1, City = model.City, UpdateTime = DateTime.Now, CreateTime = DateTime.Now };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "管理员");

                return Json(new { Flag = true, Content = "注册成功！！！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Flag = false, Content = "注册失败！！！" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}