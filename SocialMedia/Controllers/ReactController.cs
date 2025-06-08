using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Models;
using SocialMedia.Repos;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class ReactController : BaseApiController
    {
        private readonly IReactRepo reactRepo;
        private readonly IApplicationUserRepo userRepo;
        private readonly IMapper mapper;
        private readonly IPostRepo postRepo;

        public ReactController(IReactRepo reactRepo, IApplicationUserRepo userRepo, IMapper mapper, IPostRepo postRepo)
        {
            this.reactRepo = reactRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
            this.postRepo = postRepo;
        }

        [HttpPost("{postId:int}")]
        public async Task<IActionResult> AddReactToPost(int postId, AddReactDto reactDto)
        {
            var post = await postRepo.GetPostAync(postId);
            if (post == null) { return NotFound("No Post Found"); }


            var user = await userRepo.GetUserByIdAsync(User.GetCurrentUserID());

            var existedReact = user.Reactions.Where(p => p.postId == postId && p.applicationUserId == User.GetCurrentUserID()).FirstOrDefault();
            if (existedReact != null)
            {
                existedReact.Type = reactDto.Type;
                existedReact.ReactedAt = DateTime.UtcNow;

                var result = await reactRepo.SaveAllChangesAsync();

                if (result > 0)
                    return Ok("React updated Successfully");
                else
                    return BadRequest("Failed To change this react");
            }
            else
            {
                var react = new Reaction
                {
                    ReactedAt = DateTime.UtcNow,
                    postId = postId,
                    applicationUserId = User.GetCurrentUserID(),
                    Type = reactDto.Type,
                };

                reactRepo.AddReact(react);
                var result = await reactRepo.SaveAllChangesAsync();

                if (result > 0)
                    return Ok("React added to post Successfully");
                else
                    return BadRequest("Failed To Add React to this post");
            }
        }

        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> DeleteReactFromPost(int postId)
        {
            var post = await postRepo.GetPostAync(postId);
            if (post == null) { return NotFound("No post found with this id"); }
            var reacts = post.Reactions;
            var reactForCurrentUserAtPost = reacts.Where(r => r.postId == postId && r.applicationUserId == User.GetCurrentUserID()).FirstOrDefault();
            if (reactForCurrentUserAtPost != null)
            {
                reactRepo.DeleteReact(reactForCurrentUserAtPost);
                var result = await reactRepo.SaveAllChangesAsync();

                if (result > 0) return Ok("React deleted Successfully");
                else
                    return BadRequest("Failed To delete this react");
            }
            return NotFound("No react found for this post and this user");
        }
    }
}
