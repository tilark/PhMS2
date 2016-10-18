using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PHMS2.Startup))]
namespace PHMS2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
