using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [Authorize]
    public class FormController : Controller
    {
        private readonly IFormService formService;
        private readonly IQuestionService questionService;

        public FormController(IFormService formService, IQuestionService questionService)
        {
            this.formService = formService;
            this.questionService = questionService;
        }

        [HttpGet]
        public IActionResult Create(Guid templateId)
        {
            ViewBag.TemplateId = templateId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid templateId, List<FormWithQuestionsViewModel> forms)
        {
            if (ModelState.IsValid)
            {
                foreach (var formModel in forms)
                {
                    var form = formService.CreateForm(templateId, formModel);
                    await formService.SaveFormAsync(form);
                    var question = questionService.CreateQuestion(formModel.Question, form.Id);
                    await questionService.SaveQuestionAsync(question);
                }
                return RedirectToAction("MainPage", "MainPage");
            }
            ViewBag.TemplateId = templateId;
            return View(forms);
        }

        [HttpGet]
        public IActionResult CreateQuestion(Guid formId)
        {
            ViewBag.FormId = formId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(Guid formId, QuestionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = questionService.CreateQuestion(model, formId);
                await questionService.SaveQuestionAsync(question);
            }
            ViewBag.FormId = formId;
            return View(model);
        }
    }
}
