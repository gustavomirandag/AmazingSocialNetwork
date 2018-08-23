using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialNetworkWebApp.Startup))]
namespace SocialNetworkWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
