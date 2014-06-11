using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using YetAnotherTodo.Mvc.Models;

namespace YetAnotherTodo.Mvc.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        // GET: Todo
        public ActionResult Index()
        {
            ViewBag.BaseApiUrl = ConfigurationManager.AppSettings["WebApiUri"];
            ViewBag.AuthToken = User.Identity.Name;

            return View();
        }

        public async Task<ActionResult> TestApi()
        {
            //Example call of the Web API from within MVC.
            //User.Identity.Name contains the bearer token if you've set it in the AccountController (using FormsAuthentication.SetAuthCookie)
            var result = await WebApiService.Instance.GetAsync<ICollection<TodoItem>>("/api/TodoItem", User.Identity.Name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}