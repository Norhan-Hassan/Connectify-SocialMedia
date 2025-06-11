using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public interface IFollowRepo
    {
        Task<UserFollow> GetUserFollowAsync(string followerId, string FolloweeId);
        Task<PagedList<FollowDto>> GetUserFollowAsync(FollowParams followParams);
        Task<ApplicationUser> GetUserWithFollowsAsync(string userId);
    }
}
