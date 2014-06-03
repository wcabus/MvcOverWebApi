using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YetAnotherTodo.WebApi.Startup))]

namespace YetAnotherTodo.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
