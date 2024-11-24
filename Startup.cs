using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Asp_Web_Lib.Startup))]
namespace Asp_Web_Lib
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
