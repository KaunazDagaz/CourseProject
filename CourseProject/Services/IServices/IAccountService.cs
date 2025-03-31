using CourseProject.Entities;
using CourseProject.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Services.IServices
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<(SignInResult Result, User? User)> LoginUserAsync(LoginViewModel model);
    }
}
