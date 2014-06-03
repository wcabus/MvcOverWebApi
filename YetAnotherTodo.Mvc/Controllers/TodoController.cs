using System.Web.Mvc;

namespace YetAnotherTodo.Mvc.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        // GET: Todo
        public ActionResult Index()
        {
            return View();
        }
    }
}