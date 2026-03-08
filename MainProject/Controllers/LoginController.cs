using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository.Users;

namespace MainProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUser _user;
        public LoginController(IUser user)
        {
            _user = user;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _user.ValidateUser(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email or Password");
                return View(model);
            }

            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("UserId", user.Id);

            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Dashboard", "Employee");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
