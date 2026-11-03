using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using TayanaYacht.Models;

namespace TayanaYacht.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            var user = db.AdminUsers.FirstOrDefault(u => u.Username == username);

            if (user == null || string.IsNullOrEmpty(password)
                || !Crypto.VerifyHashedPassword(user.PasswordHash, password))
            {
                ViewBag.Error = "帳號或密碼錯誤";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            FormsAuthentication.SetAuthCookie(user.Username, false);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}