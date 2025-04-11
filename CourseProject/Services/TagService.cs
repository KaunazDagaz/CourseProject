using CourseProject.Entities;
using CourseProject.Services.IServices;

namespace CourseProject.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext dbContext;

        public TagService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Tag> GetAllTags()
        {
            return dbContext.Tags
                .OrderBy(t => t.Name)
                .ToList();
        }
    }
}
