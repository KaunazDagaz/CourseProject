﻿using CourseProject.Entities;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseProject.Services
{
    public class UserValidationService : IUserValidationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public UserValidationService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<bool> IsCurrentUserValidAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            return currentUser != null && !currentUser.IsBlocked;
        }

        public async Task<bool> IsCurrentUserAdminAsync()
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
