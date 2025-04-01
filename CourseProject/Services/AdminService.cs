using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseProject.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public AdminService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = await dbContext.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            var userViewModels = mapper.Map<List<UserViewModel>>(users);
            return userViewModels;
        }

        public async Task<bool> IsCurrentUserValidAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            return currentUser != null && !currentUser.IsBlocked &&
                await userManager.IsInRoleAsync(currentUser, "Administrator");
        }

        public async Task<bool> IsCurrentUserIncludedAsync(List<string> userIds)
        {
            var currentUser = await GetCurrentUserAsync();
            return currentUser != null && userIds.Contains(currentUser.Id);
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return null;
            }
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
