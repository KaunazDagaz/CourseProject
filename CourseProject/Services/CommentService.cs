using AutoMapper;
using CourseProject.Entities;
using CourseProject.Hubs;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IHubContext<CommentsHub> hubContext;

        public CommentService(ApplicationDbContext dbContext, IMapper mapper,  IHubContext<CommentsHub> hubContext)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }

        public async Task<List<CommentViewModel>> GetCommentsAsync(Guid templateId)
        {
            var comments = await dbContext.Comments.Where(c => c.TemplateId == templateId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            return mapper.Map<List<CommentViewModel>>(comments);
        }

        public async Task<(bool success, Guid commentId, DateTime timestamp)> AddCommentAsync(CommentCreateViewModel model, string userId, string authorName)
        {
            var comment = await CreateComment(model, userId, authorName);
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
            await NotifyClientsAsync(comment, authorName, "ReceiveComment");
            return (true, comment.Id, comment.CreatedAt);
        }

        private async Task<Comment> CreateComment(CommentCreateViewModel model, string userId, string authorName)
        {
            return await Task.Run(() =>
            {
                var comment = mapper.Map<Comment>(model);
                var now = DateTime.UtcNow;
                comment.Id = Guid.NewGuid();
                comment.AuthorId = userId;
                comment.AuthorName = authorName;
                comment.CreatedAt = now;
                return comment;
            });
        }

        private async Task NotifyClientsAsync(Comment comment, string authorName, string methodName)
        {
            await hubContext.Clients.Group($"Template_{comment.TemplateId}")
                .SendAsync(methodName,
                    comment.Id.ToString(),
                    authorName,
                    comment.Content,
                    comment.CreatedAt.ToString("g"));
        }
    }
}
