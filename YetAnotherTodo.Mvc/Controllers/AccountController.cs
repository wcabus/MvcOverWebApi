using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using YetAnotherTodo.Mvc.Filters;
using YetAnotherTodo.Mvc.Models;

namespace YetAnotherTodo.Mvc.Controllers
{
    [HandleApiError]
    public class AccountController : ApiController
    {
        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await WebApiService.Instance.PostAsync("/api/Account/Register", model);
                return View("Registered");
            }
            catch (ApiException ex)
            {
                //No 200 OK result, what went wrong?
                HandleBadRequest(ex);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                throw;
            }
        }

        // GET: Account/SignIn
        public ActionResult SignIn()
        {
            return View();
        }

        // POST: Account/SignIn
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInModel model, string redirectUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await WebApiService.Instance.AuthenticateAsync<SignInResult>(model.Email, model.Password);
                
                //Remember the accesstoken in the Forms Auth cookie.
                FormsAuthentication.SetAuthCookie(result.AccessToken, model.RememberMe);
                return Redirect(redirectUrl ?? "/");
            }
            catch (ApiException ex)
            {
                //No 200 OK result, what went wrong?
                HandleBadRequest(ex);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                throw;
            }
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}