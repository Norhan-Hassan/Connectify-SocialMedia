using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public class FollowRepo : IFollowRepo
    {
        private readonly ApplicationDbContext _context;

        public FollowRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserFollow> GetUserFollowAsync(string followerId, string FolloweeId)
        {
            var userFollow = await _context.Follows.FindAsync(followerId, FolloweeId);
            return userFollow;
        }

        public async Task<PagedList<FollowDto>> GetUserFollowAsync(FollowParams followParams)
        {
            var users = _context.ApplicationUsers.OrderBy(u => u.UserName).AsQueryable();
            var follows = _context.Follows.AsQueryable();
            if (followParams.Predicate.ToLower() == "following")
            {
                follows = follows.Where(p => p.FollowerId == followParams.UserId);
                users = follows.Select(p => p.Followee);
            }

            if (followParams.Predicate.ToLower() == "followers")
            {
                follows = follows.Where(p => p.FolloweeId == followParams.UserId);
                users = follows.Select(p => p.Follower);
            }
            var retunedUsers = users.Select(user => new FollowDto
            {
                UserName = user.UserName,
                PhotoUrl = user.Photos.FirstOrDefault(u => u.IsMainPhoto == true).PhotoUrl

            });
            return PagedList<FollowDto>.Create(retunedUsers, followParams.PageNumber, followParams.PageSize);

        }

        public async Task<ApplicationUser> GetUserWithFollowsAsync(string userId)
        {
            return await _context.ApplicationUsers.Include(u => u.Followees)
                                                   .Include(u => u.Followers)
                                                   .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
