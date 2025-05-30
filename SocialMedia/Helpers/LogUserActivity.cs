using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Extensions;
using SocialMedia.Models;
using SocialMedia.Repositories;

namespace SocialMedia.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (resultContext.HttpContext.User.Identity.IsAuthenticated == false)
            {
                return;
            }
            var userName = resultContext.HttpContext.User.GetCurrentUserName();
            var userRepo = resultContext.HttpContext.RequestServices.GetService<IApplicationUserRepo>();
            ApplicationUser user = await userRepo.GetUserByNameAsync(userName);
            user.LastActive = DateTime.UtcNow;
            await userRepo.SaveAllChangesAsync();
        }
    }
}
