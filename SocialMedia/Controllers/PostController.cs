using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Repos;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class PostController : BaseApiController
    {
        private readonly IPostRepo _postRepo;
        private readonly IMapper _mapper;
        public PostController(IPostRepo postRepo, IMapper mapper)
        {
            _postRepo = postRepo;
            _mapper = mapper;
        }

        //[HttpPost]

        //public async Task<IActionResult> AddNewPost(PostDto postDto)
        //{
        //    var post = _mapper.Map<Post>(postDto);

        //    post.applicationUserId = User.GetCurrentUserID();

        //    _postRepo.AddPost(post);

        //    if (await _postRepo.SaveAllChangesAsync() > 0)
        //        return Ok("Post added successfully");
        //    else
        //        return BadRequest("Error in saving post");
        //}

    }
}
