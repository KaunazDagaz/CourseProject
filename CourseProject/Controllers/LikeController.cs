using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikeService likeService;
        private readonly IUserValidationService userValidationService;

        public LikeController(ILikeService likeService, IUserValidationService userValidationService)
        {
            this.likeService = likeService;
            this.userValidationService = userValidationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(Guid templateId)
        {
            return await HandleUserActionAsync(async () =>
            {
                var user = await userValidationService.GetCurrentUserAsync();
                string userId = user!.Id;
                var result = await likeService.ToggleLikeAsync(templateId, userId);
                if (!result.success)
                {
                    return NotFound();
                }
                return Json(new
                {
                    result.success,
                    result.userLiked,
                });
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
