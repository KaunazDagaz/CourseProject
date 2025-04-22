using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;

namespace CourseProject.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly Cloudinary cloudinary;
        private readonly IMapper mapper;

        public TemplateService(ApplicationDbContext dbContext, Cloudinary cloudinary, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.cloudinary = cloudinary;
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

        public async Task<Template> CreateTemplateAsync(TemplateCreateViewModel templateViewModel, string userId)
        {
            var template = mapper.Map<Template>(templateViewModel);
            template.Id = Guid.NewGuid();
            template.AuthorId = userId;
            template.CreatedAt = DateTime.UtcNow;
            template.UpdatedAt = DateTime.UtcNow;
            if (templateViewModel.Image != null)
            {
                string imageUrl = await UploadToCloudinaryAsync(templateViewModel.Image);
                template.Image = imageUrl;
            }
            return template;
        }

        public async Task SaveTemplateAsync(Template template)
        {
            dbContext.Templates.Add(template);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TemplateViewModel> GetTemplateAsync(Guid id)
        {
            var template = await dbContext.Templates.FindAsync(id);
            return mapper.Map<TemplateViewModel>(template);
        }

        private async Task<string> UploadToCloudinaryAsync(IFormFile image)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, await ConvertFormFileToStreamAsync(image)),
                Folder = "templates",
                PublicId = Path.GetFileNameWithoutExtension(fileName)
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        private async Task<Stream> ConvertFormFileToStreamAsync(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
