using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Extensions;
using SocialMedia.Helpers;
using SocialMedia.Models;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class FollowController : BaseApiController
    {
        #region fields
        private readonly IFollowRepo _followRepo;
        private readonly IApplicationUserRepo _userRepo;
        #endregion

        #region Constructor
        public FollowController(IFollowRepo followRepo, IApplicationUserRepo userRepo)
        {
            _followRepo = followRepo;
            _userRepo = userRepo;
        }
        #endregion

        #region endpoints


        [HttpPost("followUnFollow/{userName:alpha}")]

        public async Task<IActionResult> FollowUnFollow(string userName)
        {
            var followerUserId = User.GetCurrentUserID();
            var followerUser = await _followRepo.GetUserWithFollowsAsync(followerUserId);

            var followedUser = await _userRepo.GetUserByNameAsync(userName);


            if (followedUser == null) { return NotFound("Target User is not found "); }

            if (followerUser.Id == followedUser.Id)
            {
                return BadRequest("You can't follow yourself");
            }

            var userFollow = await _followRepo.GetUserFollowAsync(followerUserId, followedUser.Id);

            if (userFollow != null)
            {
                followerUser.Followees.Remove(userFollow);

                if (await _userRepo.SaveAllChangesAsync() > 0)
                    return Ok("You unfollowed this user");

                return BadRequest("Failed to unfollow this user");

            }

            userFollow = new UserFollow()
            {
                FollowerId = followerUserId,
                FolloweeId = followedUser.Id
            };


            followerUser.Followees.Add(userFollow);

            if (await _userRepo.SaveAllChangesAsync() > 0)
            {
                return Ok("You are Following this user now");
            }

            return BadRequest("Failed To Follow this user");
        }


        [HttpGet("following-followers")]
        public async Task<IActionResult> GetUserFollowers([FromQuery] FollowParams followParams)
        {
            var currentUserId = User.GetCurrentUserID();
            followParams.UserId = currentUserId;
            var users = await _followRepo.GetUserFollowAsync(followParams);
            Response.AddPagenationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            if (users == null) { return NotFound("No Followers For This user"); }
            return Ok(users);
        }



        #endregion


    }
}
