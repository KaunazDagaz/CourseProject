using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ITemplateService templateService;
        private readonly IUserValidationService userValidationService;

        public ProfileController(ITemplateService templateService, IUserValidationService userValidationService)
        {
            this.templateService = templateService;
            this.userValidationService = userValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            return await HandleUserActionAsync(async () =>
            {
                var user = await userValidationService.GetCurrentUserAsync();
                var profileViewModel = new ProfileViewModel
                {
                    UserId = user!.Id,
                    Name = user.Name,
                    CreatedTemplates = await templateService.GetUserCreatedTemplatesAsync(user.Id),
                    FilledTemplates = await templateService.GetUserFilledTemplatesAsync(user.Id)
                };
                return View(profileViewModel);
            });
        }

        private async Task<IActionResult> HandleUserActionAsync(Func<Task<IActionResult>> action)
        {
            if (!await userValidationService.IsCurrentUserValidAsync())
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Account") });
            }
            return await action();
        }
    }
}
