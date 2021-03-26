using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeApp1.Startup))]
namespace WeApp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
