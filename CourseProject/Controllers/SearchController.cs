using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        private readonly IUserValidationService userValidationService;

        public SearchController(ISearchService searchService, IUserValidationService userValidationService)
        {
            this.searchService = searchService;
            this.userValidationService = userValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchPage(string query, bool includePrivate = false, int limit = 50,
            double similarityThreshold = 0.1)
        {
            ViewBag.Query = query;
            ViewBag.IncludePrivate = includePrivate;
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<TemplateGalleryViewModel>());
            }
            var user = await userValidationService.GetCurrentUserAsync();
            var results = await searchService.SearchTemplatesAsync(query, includePrivate, user?.Id, limit, similarityThreshold);
            return View(results);
        }
    }
}