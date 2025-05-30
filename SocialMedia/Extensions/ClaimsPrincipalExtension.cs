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

        public static string GetCurrentUserID(this ClaimsPrincipal user)
        {
            var userID = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userID ?? string.Empty;
        }
    }
}
