using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public interface IPokesRepo
    {
        Task<UserPoke> GetUserPokeAsync(string sourceUserId, string pokedUserId);
        //Task<IEnumerable<PokeDto>> GetUserPokesAsync(string predicate, string userId);
        Task<ApplicationUser> GetUserWithLikes(string userId);
    }
}
