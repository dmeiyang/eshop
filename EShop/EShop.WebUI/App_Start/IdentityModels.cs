﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EShop.WebUI
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据
    public class ApplicationUser : IdentityUser
    {
        public virtual string City { get; set; }

        public virtual int Age { get; set; }

        public ApplicationUser()
        {
            this.Id = System.Guid.NewGuid().ToString("N");
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    // 可以通过向 ApplicationRole 类添加更多属性来为角色添加配置文件数据
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
            this.Id = System.Guid.NewGuid().ToString("N");
        }
    }

    // 可以通过 ApplicationDbContext 链接数据库并管理数据库
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}