using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISAR.Startup))]
namespace ISAR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
