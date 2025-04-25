using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerService answerService;
        private readonly IUserValidationService userValidationService;

        public AnswerController(IAnswerService answerService, IUserValidationService userValidationService)
        {
            this.answerService = answerService;
            this.userValidationService = userValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> Submit(Guid templateId)
        {
            var model = await answerService.GetTemplateForSubmissionAsync(templateId);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Submit(Guid templateId, List<AnswerSubmissionViewModel> answers)
        {
            return await HandleUserActionAsync(async () =>
            {
                var user = await userValidationService.GetCurrentUserAsync();
                await answerService.SaveAnswersAsync(answers, user!.Id);
                return RedirectToAction("ThankYou", new { templateId });
            });          
        }

        [Authorize]
        [HttpGet]
        public IActionResult ThankYou(Guid templateId)
        {
            ViewBag.TemplateId = templateId;
            return View();
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