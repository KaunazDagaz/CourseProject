using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class MainPageController : Controller
    {
        private readonly ITemplateService templateService;
        private readonly ITagService tagService;

        public MainPageController(ITemplateService templateService, ITagService tagService)
        {
            this.templateService = templateService;
            this.tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> MainPage()
        {
            var viewModel = new MainPageViewModel
            {
                LatestTemplates = await templateService.GetLatestsTemplatesAsync(6),
                PopularTemplates = await templateService.GetPopularTemplatesAsync(6),
                Tags = await tagService.GetPopularTagsAsync()
            };
            return View(viewModel);
        }
    }
}
