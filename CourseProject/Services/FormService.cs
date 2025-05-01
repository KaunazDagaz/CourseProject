using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class FormService : IFormService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public FormService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<Form>> GetAllFormsAsync(Guid templateId)
        {
            return await dbContext.Forms.ToListAsync();
        }

        public async Task<Form> CreateForm(Guid templateId, FormWithQuestionsViewModel formViewModel)
        {
            return await Task.Run(() =>
            {
                var form = mapper.Map<Form>(formViewModel);
                form.Id = Guid.NewGuid();
                form.TemplateId = templateId;
                return form;
            });
        }

        public async Task SaveFormAsync(Form form)
        {
            await dbContext.Forms.AddAsync(form);
            await dbContext.SaveChangesAsync();
        }
    }
}
