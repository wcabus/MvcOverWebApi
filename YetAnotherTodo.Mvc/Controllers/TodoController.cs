using System.Configuration;
using System.Web.Mvc;

namespace YetAnotherTodo.Mvc.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        // GET: Todo
        public ActionResult Index()
        {
            ViewBag.BaseApiUrl = ConfigurationManager.AppSettings["WebApiUri"];
            return View();
        }
    }
}