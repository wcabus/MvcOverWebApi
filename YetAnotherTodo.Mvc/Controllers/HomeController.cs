using System.Web.Mvc;

namespace YetAnotherTodo.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}