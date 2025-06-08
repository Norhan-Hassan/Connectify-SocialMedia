using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Models;
using SocialMedia.Repos;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    public class PostController : BaseApiController
    {
        private readonly IPostRepo _postRepo;
        private readonly IMapper _mapper;
        private readonly IApplicationUserRepo _userRepo;
        public PostController(IPostRepo postRepo, IMapper mapper, IApplicationUserRepo applicationUser)
        {
            _postRepo = postRepo;
            _mapper = mapper;
            _userRepo = applicationUser;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            var currentUserID = User.GetCurrentUserID();

            var user = await _userRepo.GetUserByIdAsync(currentUserID);

            var post = new Post()
            {
                Content = createPostDto.Content,
                IsAnonymous = createPostDto.IsAnonymous,
                applicationUserId = currentUserID,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
            };

            _postRepo.AddPost(post);
            var result = await _postRepo.SaveAllChangesAsync();


            if (result > 0)
            {
                var postResult = new PostDto()
                {
                    UserName = (post.IsAnonymous == true) ? "Anonymous User" : User.GetCurrentUserName(),
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    Content = post.Content
                };
                return Ok(postResult);
            }
            else
            { return BadRequest("Failed To Add Post"); }
        }

        [HttpGet("{postId:int}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            var post = await _postRepo.GetPostAync(postId);
            if (post == null) return NotFound("No Post with this id");
            else
            {
                var postDto = _mapper.Map<PostDto>(post);
                return Ok(postDto);
            }
        }
        [HttpGet("List/currentUser")]
        public async Task<IActionResult> GetPostsForCurrentUser()
        {
            var userName = User.GetCurrentUserName();
            var posts = await _postRepo.GetPostsForUserAsync(userName);
            if (posts == null) return NotFound("No Posts for this user");
            else
            {
                return Ok(posts);
            }
        }

        [HttpGet("List/{userName:alpha}")]
        public async Task<IActionResult> GetPostsForUser(string userName)
        {
            var posts = await _postRepo.GetPostsForUserAsync(userName);
            if (posts == null) return NotFound("No Posts for this user");
            else
            {
                return Ok(posts);
            }
        }


        [HttpPut("{postId:int}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostDto postDto)
        {
            var currentUserId = User.GetCurrentUserID();




            await _postRepo.Update(postId, postDto);

            var result = await _postRepo.SaveAllChangesAsync();

            if (result > 0)
            {
                return Ok("Post Updated Successfully");

            }
            return BadRequest("Failed To Update This Post");

        }

        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var post = await _postRepo.GetPostForUserAsync(User.GetCurrentUserID(), postId);
            if (post == null) return NotFound("Not able to find this post");
            else
            {
                _postRepo.DeletePost(post);
                return Ok("Post Deleted Successfully");
            }

        }
    }
}
