using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class TemplateController : Controller
    {
        private readonly IUserValidationService userValidationService;
        private readonly ITemplateService templateService;

        public TemplateController(IUserValidationService userValidationService, ITemplateService templateService)
        {
            this.userValidationService = userValidationService;
            this.templateService = templateService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return await HandleUserActionAsync(() =>
            {
                return Task.FromResult<IActionResult>(View());
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TemplateCreateViewModel model)
        {
            return await HandleUserActionAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var user = await userValidationService.GetCurrentUserAsync();
                    var userId = user!.Id;
                    var template = await templateService.CreateTemplateAsync(model, userId);
                    await templateService.SaveTemplateAsync(template);
                    return RedirectToAction("Create", "Form", new { templateId = template.Id });
                }
                return View(model);
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Template(Guid id)
        {
            var templateViewModel = await templateService.GetTemplateAsync(id);
            return View(templateViewModel);
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