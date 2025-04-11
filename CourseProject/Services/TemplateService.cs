using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;

namespace CourseProject.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public TemplateService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public List<TemplateGalleryViewModel> GetLatestsTemplates(int count)
        {
            var template = dbContext.Templates
                .Where(t => t.IsPublic)
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .ToList();
            var templateViewModels = mapper.Map<List<TemplateGalleryViewModel>>(template);
            return templateViewModels;
        }

        public List<TemplateTableViewModel> GetPopularTemplates(int count)
        {
            var popularTemplates = dbContext.Templates
                .Where(t => t.IsPublic)
                .Join(dbContext.TemplateStats,
                    template => template.Id,
                    stats => stats.TemplateId,
                    (template, stats) => new { Template = template, stats.AnswersCount })
                .OrderByDescending(x => x.AnswersCount)
                .Take(count)
                .Select(x => x.Template)
                .ToList();
            var templateViewModels = mapper.Map<List<TemplateTableViewModel>>(popularTemplates);
            return templateViewModels;
        }
    }
}
