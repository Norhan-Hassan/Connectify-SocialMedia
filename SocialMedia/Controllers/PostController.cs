using AutoMapper;
using SocialMedia.Repos;

namespace SocialMedia.Controllers
{
    public class PostController : BaseApiController
    {
        private readonly IPostRepo _postRepo;
        private readonly IMapper _mapper;
        public PostController(IPostRepo postRepo, IMapper mapper)
        {
            _postRepo = postRepo;
            _mapper = mapper;
        }



    }
}
