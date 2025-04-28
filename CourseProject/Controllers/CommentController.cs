using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IUserValidationService userValidationService;

        public CommentController(ICommentService commentService, IUserValidationService userValidationService)
        {
            this.commentService = commentService;
            this.userValidationService = userValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(Guid templateId)
        {
            var commentViewModels = await commentService.GetCommentsAsync(templateId);
            return PartialView("_Comments", commentViewModels);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] CommentCreateViewModel model)
        {
            return await HandleUserActionAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var user = await userValidationService.GetCurrentUserAsync();
                    var result = await commentService.AddCommentAsync(model, user!.Id, user.Name);
                    return Json(new
                    {
                        result.success,
                        commentId = result.commentId.ToString(),
                        timestamp = result.timestamp.ToString("g")
                    });
                }
                return View(model);
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
