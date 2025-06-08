using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Data;
using SocialMedia.Helpers;
using SocialMedia.Mapping;
using SocialMedia.Models;
using SocialMedia.Repos;
using SocialMedia.Repositories;
using SocialMedia.Services;
using System.Text;

namespace SocialMedia.Dependencies
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection ManageDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddTransient<IPostRepo, PostRepo>();
            services.AddTransient<IPresenceTracker, PresenceTracker>();
            services.AddTransient<IApplicationUserRepo, ApplicationUserRepo>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddTransient<IPokesRepo, PokesRepo>();
            services.AddTransient<IMessageRepo, MessageRepo>();
            services.AddTransient<IReactRepo, ReactRepo>();
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            return services;

        }

        public static IServiceCollection JWTConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:audience"],

                    IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT:key"]))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }

        public static IServiceCollection SwagerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Connectify", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Connectify",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
            });

            return services;
        }
    }
}
