using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
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
                
                //Let's keep the user authenticated in the MVC webapp.
                //By using the AccessToken, we can use User.Identity.Name in the MVC controllers to make API calls.
                FormsAuthentication.SetAuthCookie(result.AccessToken, model.RememberMe);
                
                //Create an AuthenticationTicket to generate a cookie used to authenticate against Web API.
                //But before we can do that, we need a ClaimsIdentity that can be authenticated in Web API.
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, result.UserName), //Name is the default name claim type, and UserName is the one known also in Web API.
                    new Claim(ClaimTypes.NameIdentifier, result.UserName) //If you want to use User.Identity.GetUserId in Web API, you need a NameIdentifier claim.
                };

                //Generate a new ClaimsIdentity, using the DefaultAuthenticationTypes.ApplicationCookie authenticationType.
                //This also matches what we've set up in Web API.
                var authTicket = new AuthenticationTicket(new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie), new AuthenticationProperties
                {
                    ExpiresUtc = result.Expires,
                    IsPersistent = model.RememberMe,
                    IssuedUtc = result.Issued,
                    RedirectUri = redirectUrl
                });

                //And now it's time to generate the cookie data. This is using the same code that is being used by the CookieAuthenticationMiddleware class in OWIN.
                byte[] userData = DataSerializers.Ticket.Serialize(authTicket);
                
                //Protect this user data and add the extra properties. These need to be the same as in Web API!
                byte[] protectedData = MachineKey.Protect(userData, new[] { "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware", DefaultAuthenticationTypes.ApplicationCookie, "v1" });

                //base64-encode this data.
                string protectedText = TextEncodings.Base64Url.Encode(protectedData);
                
                //And now, we have the cookie.
                Response.SetCookie(new HttpCookie("YetAnotherTodo.WebApi.Auth")
                {
                    HttpOnly = true,
                    Expires = result.Expires.UtcDateTime,
                    Value = protectedText
                });
                
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

            //Clear the auth cookie
            if (Response.Cookies["YetAnotherTodo.WebApi.Auth"] != null)
            {
                var c = new HttpCookie("YetAnotherTodo.WebApi.Auth") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(c);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}