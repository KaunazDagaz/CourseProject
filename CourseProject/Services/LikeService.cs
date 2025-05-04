using CourseProject.Entities;
using CourseProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext dbContext;

        public LikeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<(bool success, bool userLiked)> ToggleLikeAsync(Guid templateId, string userId)
        {
            if (!await dbContext.Templates.AnyAsync(t => t.Id == templateId))
                return (false, false);
            bool hasLiked = await HasUserLikedTemplateAsync(templateId, userId);
            if (!hasLiked)
            {
                await AddLikeAsync(templateId, userId);
                return (true, true);
            }
            else
            {
                await RemoveLikeAsync(templateId, userId);
                return (true, false);
            }
        }

        public async Task<bool> HasUserLikedTemplateAsync(Guid templateId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;
            return await dbContext.Likes.AnyAsync(l => l.TemplateId == templateId && l.AuthorId == userId);
        }

        public async Task<int> GetLikesCountAsync(Guid templateId)
        {
            return await dbContext.Likes.CountAsync(l => l.TemplateId == templateId);
        }

        private async Task AddLikeAsync(Guid templateId, string userId)
        {
            var like = new Like
            {
                Id = Guid.NewGuid(),
                AuthorId = userId,
                TemplateId = templateId
            };
            await dbContext.Likes.AddAsync(like);
            await dbContext.SaveChangesAsync();
        }

        private async Task RemoveLikeAsync(Guid templateId, string userId)
        {
            var existingLike = await dbContext.Likes.FirstOrDefaultAsync(l => l.TemplateId == templateId && l.AuthorId == userId);
            if (existingLike != null)
            {
                dbContext.Likes.Remove(existingLike);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}