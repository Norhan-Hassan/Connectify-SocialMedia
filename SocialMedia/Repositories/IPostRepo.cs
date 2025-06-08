using SocialMedia.DTOs;
using SocialMedia.Models;

namespace SocialMedia.Repos
{
    public interface IPostRepo
    {
        void AddPost(Post post);
        void DeletePost(Post post);
        Task<Post> GetPostAync(int postId);
        Task<IEnumerable<PostDto>> GetPostsForUserAsync(string userName);
        Task Update(int postId, UpdatePostDto postDto);
        Task<Post> GetPostForUserAsync(string userId, int postId);
        Task<int> SaveAllChangesAsync();
    }
}
