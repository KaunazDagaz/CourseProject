using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseProject.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerService answerService;
        private readonly IUserValidationService userValidationService;
        private readonly ILikeService likeService;

        public AnswerController(IAnswerService answerService, IUserValidationService userValidationService,
            ILikeService likeService)
        {
            this.answerService = answerService;
            this.userValidationService = userValidationService;
            this.likeService = likeService;
        }

        [HttpGet]
        public async Task<IActionResult> Submit(Guid templateId)
        {
            var model = await answerService.GetTemplateForSubmissionAsync(templateId);
            if (model == null)
            {
                return NotFound();
            }
            model.Forms = model.Forms.Where(f => f.ShowInTable).ToList();
            var user = await userValidationService.GetCurrentUserAsync();
            if (user != null)
            {
                string userId = user.Id;
                model.HasPreviouslyAnswered = await answerService.HasAnsweredAsync(templateId, userId);
                if (model.HasPreviouslyAnswered)
                {
                    await answerService.LoadPreviousAnswersAsync(model.Forms, userId);
                }
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
                bool hasAnswered = await answerService.HasAnsweredAsync(templateId, user!.Id);
                if (hasAnswered)
                {
                    await answerService.UpdateAnswersAsync(answers, user.Id);
                }
                else
                {
                    await answerService.SaveAnswersAsync(answers, user.Id);
                }
                return RedirectToAction("ThankYou", new { templateId });
            });          
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ThankYou(Guid templateId)
        {
            ViewBag.TemplateId = templateId;
            ViewBag.LikesCount = await likeService.GetLikesCountAsync(templateId);
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await userValidationService.GetCurrentUserAsync();
                ViewBag.UserHasLiked = await likeService.HasUserLikedTemplateAsync(templateId, user!.Id);
            }
            else
            {
                ViewBag.UserHasLiked = false;
            }
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