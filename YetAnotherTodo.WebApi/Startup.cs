using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using YetAnotherTodo.EF;
using YetAnotherTodo.EF.Repository;
using YetAnotherTodo.WebApi.Models;

[assembly: OwinStartup(typeof(YetAnotherTodo.WebApi.Startup))]

namespace YetAnotherTodo.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.CreatePerOwinContext<TodoItemRepository>((fo, ctx) => new TodoItemRepository(ctx.Get<YetAnotherTodoDbContext<ApplicationUser>>()));
        }
    }
}
