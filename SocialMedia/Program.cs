using SocialMedia.Dependencies;
using SocialMedia.HUBs;
using SocialMedia.MiddleWare;

namespace SocialMedia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();


            builder.Services.ManageDependencies(builder.Configuration)
                            .JWTConfiguration(builder.Configuration)
                            .SwagerConfiguration();

            builder.Services.AddSignalR();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("hub/chat");
            app.MapHub<PresenceHub>("hub/presence");

            app.MapControllers();

            app.Run();
        }
    }
}
