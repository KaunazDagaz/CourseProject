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
            CreateMap<Template, TemplateGalleryViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom<AuthorNameResolver>());
            CreateMap<Template, TemplateTableViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom<AuthorNameResolver>());
            CreateMap<TemplateCreateViewModel, Template>();
            CreateMap<QuestionCreateViewModel, Question>();
            CreateMap<Question, QuestionViewModel>().ReverseMap();
            CreateMap<FormWithQuestionsViewModel, Form>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.TemplateId, opt => opt.Ignore());
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

        public class AuthorNameResolver : IValueResolver<Template, TemplateGalleryViewModel, string>, IValueResolver<Template, TemplateTableViewModel, string>
        {
            private readonly UserManager<User> userManager;

            public AuthorNameResolver(UserManager<User> userManager)
            {
                this.userManager = userManager;
            }

            public string Resolve(Template source, TemplateGalleryViewModel destination, string destMember, ResolutionContext context)
            {
                return ResolveAuthorName(source.AuthorId);
            }

            public string Resolve(Template source, TemplateTableViewModel destination, string destMember, ResolutionContext context)
            {
                return ResolveAuthorName(source.AuthorId);
            }

            private string ResolveAuthorName(string authorId)
            {
                var user = userManager.FindByIdAsync(authorId).Result;
                return user?.Name ?? "Unknown";
            }
        }
    }
}
