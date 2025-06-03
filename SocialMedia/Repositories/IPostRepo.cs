using SocialMedia.DTOs;
using SocialMedia.Models;

namespace SocialMedia.Repos
{
    public interface IPostRepo
    {
        void AddPost(Post post);
        void DeletePost(Post post);
        Task<Post> GetPostAync(int postId);
        Task<IEnumerable<PostDto>> GetPostsForUserAsync(string userId);
        Task<int> SaveAllChangesAsync();
    }
}
