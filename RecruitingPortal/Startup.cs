using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecruitingPortal.Startup))]
namespace RecruitingPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
