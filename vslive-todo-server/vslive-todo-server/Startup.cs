using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(vslive_todo_server.Startup))]

namespace vslive_todo_server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}