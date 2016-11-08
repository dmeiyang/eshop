using EShop.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EShop.WebUI.Areas.Admin.Controllers
{
    public class RoleController : BasicController
    {
        public ActionResult Index(string key, int page = 1)
        {
            ViewBag.Key = key;

            var entitys = RoleManager.Roles;

            if (!string.IsNullOrEmpty(key))
            {
                entitys = entitys.Where(x => x.Name.Contains(key));
            }

            return View(entitys.OrderBy(x => x.Name).ToPageList(page, 10));
        }

        public ActionResult Add()
        {
            ViewBag.Title = "添加角色";

            return PartialView(new ApplicationRole() { Id = null });
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Title = "编辑角色";

            var entity = await RoleManager.FindByIdAsync(id);

            if (entity == null)
                return Content("该角色不存在，非法数据或者数据已被其他管理员删除！！！");

            return PartialView("Add", entity);
        }

        [HttpPost]
        public async Task<ActionResult> Save(Models.RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return JRFaild();

            if (string.IsNullOrEmpty(model.Id))
            {
                var result = await RoleManager.CreateAsync(new ApplicationRole()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreateTime = DateTime.Now,
                });

                return JRCommonHandleResult(result.Succeeded);
            }
            else
            {
                var entity = await RoleManager.FindByIdAsync(model.Id);

                if (entity == null)
                    return JRFaild("该角色不存在，非法数据或者数据已被其他管理员删除！！！");

                entity.Name = model.Name;
                entity.Description = model.Description;

                var result = await RoleManager.UpdateAsync(entity);

                return JRCommonHandleResult(result.Succeeded);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var array = ids.ToSplit(',');

            foreach (var v in array)
            {
                var entity = await RoleManager.FindByIdAsync(v);

                if (entity != null)
                {
                    await RoleManager.DeleteAsync(entity);
                }
            }

            return JRCommonHandleResult(true);
        }
    }
}