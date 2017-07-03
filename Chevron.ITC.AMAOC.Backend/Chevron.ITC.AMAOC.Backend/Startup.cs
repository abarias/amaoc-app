using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Chevron.ITC.AMAOC.Backend.Startup))]

namespace Chevron.ITC.AMAOC.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}
