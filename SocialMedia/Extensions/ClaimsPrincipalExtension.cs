using System.Security.Claims;

namespace SocialMedia.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetCurrentUserName(this ClaimsPrincipal user)
        {
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;
            return userName ?? string.Empty;
        }
    }
}
