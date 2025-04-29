using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string tagsModel)
        {
            if (string.IsNullOrWhiteSpace(tagsModel))
                return Json(Array.Empty<object>());
            var tags = await tagService.SearchTagsAsync(tagsModel);
            return Json(tags.Select(t => new { id = t.Id, name = t.Name }));
        }
    }
}
