using BusinessLayer.Account;
using CourseTrack.Models;
using DataLayer.Entities.StudentEntity;
using Microsoft.AspNetCore.Mvc;

namespace CourseTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountFacade _accountFacade;

        public AccountController(IAccountFacade accountFacade)
        {
            _accountFacade = accountFacade;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isValid = _accountFacade.Login(model.Email, model.Password);
                if (isValid)
                {
                    var token = _accountFacade.GetToken(model.Email);
                    Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                    TempData["Error"] = null;
                    return RedirectToAction("Index", "Home");
                }
                TempData["Error"] = "Неправильний Email/Пароль";
                return View(model);
            }

            TempData["Error"] = "Неправильний Email/Пароль";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registered = _accountFacade.Register(model.FirstName, model.LastName, model.Email, model.Password);

                if (registered)
                {
                    var token = _accountFacade.GetToken(model.Email);
                    Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                    return RedirectToAction("Index", "Home");
                }

                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("X-Access-Token");
            return RedirectToAction("Index", "Home");
        }
    }
}
