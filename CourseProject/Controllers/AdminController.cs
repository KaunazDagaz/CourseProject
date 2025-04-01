using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await HandleUserAction(async () =>
            {
                var userViewModels = await adminService.GetUsers();
                return View(userViewModels);
            });
        }

        private async Task<IActionResult> HandleUserAction(Func<Task<IActionResult>> action)
        {
            if (!await adminService.IsCurrentUserValidAsync())
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return RedirectToAction("Login", "Account");
            }
            return await action();
        }

        private async Task<IActionResult> HandleUserInclusionCheck(List<string> userIds)
        {
            if (await adminService.IsCurrentUserIncludedAsync(userIds))
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return RedirectToAction("Login", "Account");
            }
            return Json(new { success = true });
        }
    }
}
