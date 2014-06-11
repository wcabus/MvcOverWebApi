using System.CodeDom;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
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
            //app.Use((ctx, next) =>
            //{
            //    if (ctx.Request != null && ctx.Request.Cookies != null)
            //    {
            //        Debug.WriteLine("===================COOKIES===============");
            //        foreach (var cookie in ctx.Request.Cookies)
            //        {
            //            Debug.WriteLine("{0} = {1}", cookie.Key, cookie.Value);
            //        }
            //        Debug.WriteLine("===================COOKIES===============");
            //    }

            //    if (next != null)
            //    {
            //        return next();
            //    }

            //    return Task.FromResult(0);
            //});

            ConfigureAuth(app);

            app.CreatePerOwinContext<TodoItemRepository>((fo, ctx) => new TodoItemRepository(ctx.Get<YetAnotherTodoDbContext<ApplicationUser>>()));
        }
    }
}
