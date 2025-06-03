using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Models;

namespace SocialMedia.Repos
{
    public class PostRepo : IPostRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PostRepo(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var post = await _context.Posts.FindAsync(postId);
            if (post == null) { return null; }
            return post;
        }

        public async Task<IEnumerable<PostDto>> GetPostsForUserAsync(string userId)
        {
            var posts = await _context.Posts
                                .Include(u => u.applicationUser)
                                .Where(u => u.applicationUserId == userId)
                                .ToListAsync();

            var mappedPosts = _mapper.Map<IEnumerable<PostDto>>(posts);
            return mappedPosts;

        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
