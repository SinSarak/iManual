using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iManual.Startup))]
namespace iManual
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
