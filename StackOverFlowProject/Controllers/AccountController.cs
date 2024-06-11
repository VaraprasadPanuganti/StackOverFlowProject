using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using StackOverFlowProject.CustomFilters;
using ViewModel;

namespace StackOverFlowProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService us;

        public AccountController(IUsersService _us)
        {
            us = _us;
        }

        [Route("account/register")]
        public IActionResult Register()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [Route("account/register")]
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                int uid = this.us.InsertUser(rvm);
                HttpContext.Session.SetInt32("CurrentUserID", uid);
                HttpContext.Session.SetString("CurrentUserName", rvm.Name);
                HttpContext.Session.SetString("CurrentUserEmail", rvm.Email);
                HttpContext.Session.SetString("CurrentUserPassword", rvm.Password);
                HttpContext.Session.SetString("CurrentUserIsAdmin", false.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View();
            }
        }

        [Route("account/login")]
        public IActionResult Login()
        {
            LoginViewModel lvm = new LoginViewModel();
            return View(lvm);
        }

        [HttpPost]
        [Route("account/login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel lvm)
            {
            if (ModelState.IsValid)
            {
                UserViewModel uvm = this.us.GetUsersByEmailAndPassword(lvm.Email, lvm.Password);
                if (uvm != null)
                {
                    HttpContext.Session.SetInt32("CurrentUserID", uvm.UserID);
                    HttpContext.Session.SetString("CurrentUserName", uvm?.Name);
                    HttpContext.Session.SetString("CurrentUserEmail", uvm?.Email);
                    // HttpContext.Session.SetString("CurrentUserPassword", uvm?.Password);
                    HttpContext.Session.SetString("CurrentUserIsAdmin", uvm.IsAdmin.ToString());
                    var password = uvm?.Password;
                    var ab = HttpContext.Session.GetString("CurrentUserName");
                    // Check if the password is not null before setting the session value
                    if (password != null)
                    {
                        HttpContext.Session.SetString("CurrentUserPassword", password);
                    }
                    else
                    {
                        // Optionally, handle the case where the password is null
                        HttpContext.Session.Remove("CurrentUserPassword"); // Remove the session key if it exists
                    }

                    if (uvm.IsAdmin)
                    {
                        return RedirectToRoute(new { area = "admin", controller = "AdminHome", action = "Index" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("x", "Invalid Email / Password");
                    return View(lvm);
                }
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(lvm);
            }
        }
        [Route("account/logout")]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [TypeFilter(typeof(UserAuthorizationFilterAttribute))]
        [Route("account/changeprofile")]
        public ActionResult ChangeProfile()
        {
            int uid = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserDetailsViewModel eudvm = new EditUserDetailsViewModel() { Name = uvm.Name, Email = uvm.Email, Mobile = uvm.Mobile, UserID = uvm.UserID };
            return View(eudvm);
        }

        [HttpPost]
        [Route("account/changeprofile")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(UserAuthorizationFilterAttribute))]
        public ActionResult ChangeProfile(EditUserDetailsViewModel eudvm)
        {
            if (ModelState.IsValid)
            {
                eudvm.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
                this.us.UpdateUserDetails(eudvm);
                HttpContext.Session.SetString("CurrentUserName", eudvm.Name);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View(eudvm);
            }
        }

        [TypeFilter(typeof(UserAuthorizationFilterAttribute))]
        [Route("account/changepassword")]
        public ActionResult ChangePassword()
        {
            int uid = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserPasswordViewModel eupvm = new EditUserPasswordViewModel() { Email = uvm.Email, Password = "", ConfirmPassword = "", UserID = uvm.UserID };
            return View(eupvm);
        }

        [HttpPost]
        [Route("account/changepassword")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(UserAuthorizationFilterAttribute))]
        public ActionResult ChangePassword(EditUserPasswordViewModel eupvm)
        {
            if (ModelState.IsValid)
            {
                eupvm.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
                this.us.UpdateUserPassword(eupvm);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View(eupvm);
            }
        }


    }
}
