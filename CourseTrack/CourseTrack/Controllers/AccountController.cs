using BusinessLayer.Account;
using BusinessLayer.Services;
using Castle.Core.Smtp;
using CourseTrack.Models;
using DataLayer.Entities.StudentEntity;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;

namespace CourseTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailService emailService1;
        private readonly IAccountFacade _accountFacade;

        public AccountController(IEmailService emailService, IAccountFacade accountFacade)
        {
            emailService1 = emailService;
            _accountFacade = accountFacade;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _accountFacade.ForgotPassword(model.Email);

                    TempData["Error"] = null;
                    TempData["Message"] = "Повідомлення успішно відправлено. Будь ласка перевірте свою пошту";
                }
                return View(model);
            }
            catch
            {
                TempData["Error"] = "Користувач з таким емейлом не зареєстрований";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult SetNewPassword([FromQuery] string token)
        {
            var userExists = _accountFacade.GetUserByToken(token);

            if (!userExists)
                TempData["Error"] = "Невалідний токен";

            return View();
        }

        [HttpPost]
        public IActionResult SetNewPassword(NewPasswordModel model)
        {
            try
            {
                _accountFacade.SetNewPassword(model.Token, model.Password);

                TempData["Error"] = null;
                TempData["Message"] = "Пароль успішно відновлено. Тепер ви можете авторизуватись";

                return View(model);
            }
            catch
            {
                return View(model);

            }
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
