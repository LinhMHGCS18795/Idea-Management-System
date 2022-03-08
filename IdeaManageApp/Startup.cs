using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdeaManageApp.Startup))]
namespace IdeaManageApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
