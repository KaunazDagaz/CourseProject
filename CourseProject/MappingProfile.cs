using AutoMapper;
using CourseProject.Entities;
using CourseProject.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseProject
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<LoginViewModel, User>();
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom<UserRoleResolver>());
        }

        public class UserRoleResolver : IValueResolver<User, UserViewModel, string>
        {
            private readonly UserManager<User> userManager;

            public UserRoleResolver(UserManager<User> userManager)
            {
                this.userManager = userManager;
            }

            public string Resolve(User source, UserViewModel destination, string destMember, ResolutionContext context)
            {
                var roles = userManager.GetRolesAsync(source).Result;
                return roles.FirstOrDefault() ?? string.Empty;
            }
        }
    }
}
