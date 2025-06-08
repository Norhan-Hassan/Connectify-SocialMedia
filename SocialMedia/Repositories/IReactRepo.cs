using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public interface IReactRepo
    {
        void AddReact(Reaction reaction);
        void DeleteReact(Reaction reaction);
        Task<int> GetReactCount(int postId);
        Task<int> SaveAllChangesAsync();
    }
}
