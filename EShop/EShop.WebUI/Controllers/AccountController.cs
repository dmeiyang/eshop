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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string account, string password)
        {
            // 1. 利用ASP.NET Identity获取用户对象
            var user = await UserManager.FindAsync(account, password);

            // 验证用户密码和登录密码是否一致
            // UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);

            if (user == null)
                return Json(new { Flag = false, Content = "用户名或密码错误！！！" });

            // 2. 利用ASP.NET Identity获取identity 对象

            // mvc自带创建identity方法
            // var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // 自定义创建identity方法
            var claims = new List<System.Security.Claims.Claim>();

            claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName));
            claims.Add(new System.Security.Claims.Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            claims.Add(new System.Security.Claims.Claim("AspNet.Identity.SecurityStamp", user.SecurityStamp));

            claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "user"));

            //声明身份验证方式
            var identity = new System.Security.Claims.ClaimsIdentity("ApplicationCookie");

            identity.AddClaims(claims);

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
            //var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

            //var result = await UserManager.CreateAsync(user, model.Password);

            //if (result.Succeeded)
            //{
            //    return Json(new { Flag = true, Content = "注册成功！！！" }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { Flag = false, Content = "注册失败！！！" }, JsonRequestBehavior.AllowGet);
            //}

            var db = new Data.DataContext();

            db.Members.Add(new Data.DomainModels.Member()
            {
                Id = Guid.NewGuid().ToString("N"),
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = model.Email,
                PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password),
                UserName = model.UserName
            });

            var result = await db.SaveChangesAsync();

            if (result > 0)
            {
                return Json(new { Flag = true, Content = "注册成功！！！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Flag = false, Content = "注册失败！！！" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}