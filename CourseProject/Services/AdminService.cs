using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public AdminService(ApplicationDbContext dbContext, UserManager<User> userManager, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var users = await dbContext.Users.OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            var userViewModels = mapper.Map<List<UserViewModel>>(users);
            return userViewModels;
        }

        public async Task UpdateUserStatusAsync(List<string> userIds, bool status)
        {
            await dbContext.Users.Where(u => userIds.Contains(u.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.IsBlocked, status));
        }

        public async Task RemoveUserAsync(List<string> userIds)
        {
            await dbContext.Users.Where(u => userIds.Contains(u.Id))
                .ExecuteDeleteAsync();
        }

        public async Task UpdateUserRoleAsync(List<string> userIds, string role)
        {
            var users = await userManager.Users.Where(u => userIds.Contains(u.Id))
                .ToListAsync();
            foreach (var user in users)
            {
                var currentRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!string.IsNullOrEmpty(role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
