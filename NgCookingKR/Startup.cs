using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NgCookingKR.Startup))]
namespace NgCookingKR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
