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
            return await HandleUserActionAsync(async () =>
            {
                var userViewModels = await adminService.GetUsersAsync();
                return View(userViewModels);
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersTable()
        {
            return await HandleUserActionAsync(async () =>
            {
                var userViewModels = await adminService.GetUsersAsync();
                return PartialView("_UsersTable", userViewModels);
            });
        }

        [HttpPost]
        public async Task<IActionResult> Block([FromBody] List<string> userIds)
        {
            return await HandleUserActionAsync(async () =>
            {
                await adminService.UpdateUserStatusAsync(userIds, true);
                return await HandleUserInclusionCheckAsync(userIds);
            });
        }

        [HttpPost]
        public async Task<IActionResult> Unblock([FromBody] List<string> userIds)
        {
            return await HandleUserActionAsync(async () =>
            {
                await adminService.UpdateUserStatusAsync(userIds, false);
                return Json(new { success = true });
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] List<string> userIds)
        {
            return await HandleUserActionAsync(async () =>
            {
                await adminService.RemoveUserAsync(userIds);
                return await HandleUserInclusionCheckAsync(userIds);
            });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdmin([FromBody] List<string> userIds)
        {
            return await HandleUserActionAsync(async () =>
            {
                await adminService.UpdateUserRoleAsync(userIds, string.Empty);
                return await HandleUserInclusionCheckAsync(userIds);
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin([FromBody] List<string> userIds)
        {
            return await HandleUserActionAsync(async () =>
            {
                await adminService.UpdateUserRoleAsync(userIds, "Administrator");
                return Json(new { success = true });
            });
        }

        private async Task<IActionResult> HandleUserActionAsync(Func<Task<IActionResult>> action)
        {
            if (!await adminService.IsCurrentUserValidAsync())
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Account") });
            }
            return await action();
        }

        private async Task<IActionResult> HandleUserInclusionCheckAsync(List<string> userIds)
        {
            if (await adminService.IsCurrentUserIncludedAsync(userIds))
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Account") });
            }
            return Json(new { success = true });
        }
    }
}
