using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await accountService.RegisterUserAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Email", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (result, user) = await accountService.LoginUserAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("MainPage", "MainPage");
                }
                GetLoginErrorMessage(user);
            }
            return View(model);
        }

        private void GetLoginErrorMessage(User? user)
        {
            if (user == null)
            {
                ModelState.AddModelError("Email", "Error.UserNotExist");
                return;
            }
            if (user.IsBlocked)
            {
                ModelState.AddModelError("Email", "Error.UserIsBlocked");
                return;
            }
            ModelState.AddModelError("Password", "Error.SomeError");
        }
    }
}
