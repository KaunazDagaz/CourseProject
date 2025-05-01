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
                    var form = await formService.CreateForm(templateId, formModel);
                    await formService.SaveFormAsync(form);
                    var question = await questionService.CreateQuestion(formModel.Question, form.Id);
                    await questionService.SaveQuestionAsync(question, formModel.Question.CheckboxOptions);
                }
                return RedirectToAction("MainPage", "MainPage");
            }
            ViewBag.TemplateId = templateId;
            return View(forms);
        }
    }
}
