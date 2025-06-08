using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Repos;
using SocialMedia.Repositories;

namespace SocialMedia.HUBs
{
    public class PostHub : Hub
    {
        #region fields
        private readonly IPostRepo _postRepo;
        private readonly IMapper _mapper;
        private readonly IApplicationUserRepo _userRepo;
        #endregion
        #region Constructor

        public PostHub(IPostRepo postRepo, IMapper mapper, IApplicationUserRepo userRepo)
        {
            _postRepo = postRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }
        #endregion

        #region Methods

        public override Task OnConnectedAsync()
        {
            var curentUserName = Context.User.GetCurrentUserName();



            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


        private void CreatePost(PostDto postDto)
        {

        }
        #endregion
    }
}
