using CourseProject.Entities;
using CourseProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext dbContext;

        public TagService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Tag>> SearchTagsAsync(string query, int limit = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Tag>();
            return await dbContext.Tags.Where(t => t.Name.ToLower().Contains(query.ToLower()))
                .OrderBy(t => t.Name)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<Tag>> GetPopularTagsAsync(int limit = 20)
        {
            return await dbContext.TemplateTags.GroupBy(tt => tt.TagId)
                .Select(g => new { TagId = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(limit)
                .Join(dbContext.Tags,
                    g => g.TagId,
                    t => t.Id,
                    (g, t) => t)
                .ToListAsync();
        }

        public async Task AddTagsToTemplateAsync(Guid templateId, IEnumerable<string> tagNames)
        {
            foreach (var tagName in tagNames.Where(t => !string.IsNullOrWhiteSpace(t)).Distinct())
            {
                var tag = await GetTagAsync(tagName.Trim());
                var templateTag = new TemplateTag
                {
                    TemplateId = templateId,
                    TagId = tag.Id
                };
                if (!await dbContext.TemplateTags.AnyAsync(tt => tt.TemplateId == templateId && tt.TagId == tag.Id))
                    await dbContext.TemplateTags.AddAsync(templateTag);
            }
            await dbContext.SaveChangesAsync();
        }

        private async Task<Tag> GetTagAsync(string tagName)
        {
            var tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
            if (tag != null)
                return tag;
            var newTag = new Tag { Id = Guid.NewGuid(), Name = tagName };
            await dbContext.Tags.AddAsync(newTag);
            await dbContext.SaveChangesAsync();
            return newTag;
        }
    }
}
