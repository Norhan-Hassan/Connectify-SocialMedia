using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Models;
using SocialMedia.Repositories;

namespace SocialMedia.Repos
{
    public class PostRepo : IPostRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IApplicationUserRepo _userRepo;
        private readonly IMapper _mapper;

        public PostRepo(ApplicationDbContext context, IMapper mapper, IApplicationUserRepo userRepo)
        {
            _context = context;
            _mapper = mapper;
            _userRepo = userRepo;
        }
        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
        }

        public void DeletePost(Post post)
        {
            _context.Posts?.Remove(post);
        }

        public async Task<Post> GetPostAync(int postId)
        {
            var post = await _context.Posts.Include(p => p.applicationUser).FirstOrDefaultAsync(p => p.ID == postId);
            if (post == null) { return null; }
            return post;
        }

        public async Task<IEnumerable<PostDto>> GetPostsForUserAsync(string userName)
        {
            var user = await _userRepo.GetUserByNameAsync(userName);

            var posts = await _context.Posts
                                .Include(u => u.applicationUser)
                                .Where(p => p.applicationUserId == user.Id).ToListAsync();


            var mappedPosts = _mapper.Map<IEnumerable<PostDto>>(posts);

            return mappedPosts;

        }

        public async Task<Post> GetPostForUserAsync(string userId, int postId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            var userpost = user.Posts.Where(p => p.ID == postId).FirstOrDefault();
            return userpost;
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task Update(int postId, UpdatePostDto postDto)
        {
            var postInDb = await _context.Posts.Where(p => p.ID == postId).FirstOrDefaultAsync();
            if (postInDb.UpdatedAt < DateTime.UtcNow || postInDb.UpdatedAt == null)
            {
                postInDb.UpdatedAt = DateTime.UtcNow;
                postInDb.Content = postDto.Content;
            }
        }
    }
}
