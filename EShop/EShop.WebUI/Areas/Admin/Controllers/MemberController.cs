using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EShop.WebUI.Areas.Admin.Controllers
{
    public class MemberController : BasicController
    {
        public ActionResult Index(string key, int page = 1)
        {
            ViewBag.Key = key;

            var entitys = UserManager.Users.Where(x => x.Type == 0);

            if (!string.IsNullOrEmpty(key))
            {
                entitys = entitys.Where(x => x.UserName.Contains(key));
            }

            return View(entitys.OrderByDescending(x => x.CreateTime).ToPageList(page, 10));
        }

        public ActionResult Add()
        {
            ViewBag.Title = "添加用户";

            return PartialView(new ApplicationUser() { Id = null });
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Title = "编辑用户";

            var entity = await UserManager.FindByIdAsync(id);

            if (entity == null)
                return Content("该用户不存在，非法数据或者数据已被其他管理员删除！！！");

            return PartialView("Add", entity);
        }

        [HttpPost]
        public async Task<ActionResult> Save(Models.MemberViewModel model)
        {
            if (!ModelState.IsValid)
                return JRFaild();

            if (string.IsNullOrEmpty(model.Id))
            {
                var user = new ApplicationUser()
                {
                    Id = IDHelper.Id32,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    Type = 0,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    await UserManager.AddToRoleAsync(user.Id, "普通会员");

                return JRCommonHandleResult(result.Succeeded);
            }
            else
            {
                var entity = await UserManager.FindByIdAsync(model.Id);

                if (entity == null)
                    return JRFaild("该用户不存在，非法数据或者数据已被其他管理员删除！！！");

                entity.UserName = model.UserName;
                entity.Email = model.Email;
                entity.PhoneNumber = model.PhoneNumber;
                entity.City = model.City;

                var result = await UserManager.UpdateAsync(entity);

                return JRCommonHandleResult(result.Succeeded);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var array = ids.ToSplit(',');

            foreach (var v in array)
            {
                var entity = await UserManager.FindByIdAsync(v);

                if (entity != null)
                {
                    await UserManager.DeleteAsync(entity);
                }
            }

            return JRCommonHandleResult(true);
        }
    }
}