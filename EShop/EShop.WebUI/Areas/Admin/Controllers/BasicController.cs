using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EShop.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "管理员")]
    public class BasicController : Controller
    {
        protected IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        protected ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }

        protected ApplicationRoleManager RoleManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); } }

        #region 消息提醒
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        protected ActionResult JREnumHandleResult<T>(object value)
        {
            string content = EnumDescription.GetFieldText((T)value);

            return value.ToString().Contains("Success") ? JRSuccess(content) : JRFaild("all", content);
        }

        protected ActionResult JRCommonHandleResult(bool result)
        {
            if (result)
                return JRSuccess("操作成功");
            else
                return JRFaild("all", "操作失败");
        }

        protected ActionResult JRSuccess(string obj)
        {
            return Json(new { Flag = true, Content = obj }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult JRFaild(string message)
        {
            return JRFaild("all", message);
        }

        protected ActionResult JRFaild(string key, string message)
        {
            return Content(Faild(key, message));
        }

        protected ActionResult JRFaild()
        {
            return Content(Faild());
        }

        /// <summary>
        /// 全局错误key=all，某个字段错误key=filed
        /// </summary>
        protected string Faild(string key, string message)
        {
            string[][] error = new string[1][];
            error[0] = new string[2];
            error[0][0] = key;
            error[0][1] = message;
            return serializer.Serialize(new MCJsonResult() { Flag = false, Content = error });
        }

        protected string Faild()
        {
            var errorInfo = ModelState.ToArray().Where(x => x.Value.Errors.Count > 0).ToArray();
            string[][] error = new string[errorInfo.Length][];
            for (int i = 0; i < error.Length; i++)
            {
                if (errorInfo[i].Value.Errors.Count == 0)
                    continue;
                error[i] = new string[2];
                error[i][0] = errorInfo[i].Key;
                error[i][1] = errorInfo[i].Value.Errors[0].ErrorMessage;
            }
            return serializer.Serialize(new MCJsonResult() { Flag = false, Content = error });
        }
        #endregion

        public class MCJsonResult
        {
            public bool Flag { get; set; }
            public object Content { get; set; }
        }
    }


}