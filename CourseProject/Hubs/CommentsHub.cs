using Microsoft.AspNetCore.SignalR;

namespace CourseProject.Hubs
{
    public class CommentsHub : Hub
    {
        public async Task JoinTemplateGroup(string templateId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Template_{templateId}");
        }

        public async Task LeaveTemplateGroup(string templateId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Template_{templateId}");
        }

        public async Task SendComment(string templateId, string commentId, string authorName, string content, string timestamp)
        {
            await Clients.Group($"Template_{templateId}").SendAsync("ReceiveComment", commentId, authorName, content, timestamp);
        }
    }
}
