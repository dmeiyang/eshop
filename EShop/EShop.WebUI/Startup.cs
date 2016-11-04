using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EShop.WebUI.Startup))]
namespace EShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
