using SocialMedia.Repos;
using SocialMedia.Repositories;
using System.Reflection;

namespace SocialMedia.Dependencies
{
    public static class ApiDependencyInjectionConfig
    {
        public static IServiceCollection ManageDependencies(this IServiceCollection services)
        {
            services.AddTransient<IPostRepo, PostRepo>();
            services.AddTransient<IChatRepo, ChatRepo>();
            services.AddTransient<IApplicationUserRepo, ApplicationUserRepo>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;

        }
    }
}
