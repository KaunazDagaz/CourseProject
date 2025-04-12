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
        public IActionResult MainPage()
        {
            var viewModel = new MainPageViewModel
            {
                LatestTemplates = templateService.GetLatestsTemplates(6),
                PopularTemplates = templateService.GetPopularTemplates(6),
                Tags = tagService.GetAllTags()
            };

            return View(viewModel);
        }
    }
}
