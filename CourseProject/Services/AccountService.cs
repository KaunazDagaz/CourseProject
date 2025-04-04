using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            var user = mapper.Map<User>(model);
            var result = await userManager.CreateAsync(user, model.Password);
            await AddAdminRole(user);
            return result;
        }

        public async Task<(SignInResult Result, User? User)> LoginUserAsync(LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && user.IsBlocked)
            {
                return (SignInResult.Failed, user);
            }
            var result = await signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: true);
            return (result, user);
        }

        private async Task AddAdminRole(User user)
        {
            var r = await roleManager.FindByNameAsync("Administrator");
            if (r == null)
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }
}
