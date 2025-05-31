using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public interface IPokesRepo
    {
        Task<UserPoke> GetUserPokeAsync(string sourceUserId, string pokedUserId);
        Task<PagedList<PokeDto>> GetUserPokesAsync(PokesParams pokesParams);
        Task<ApplicationUser> GetUserWithPokesAsync(string userId);
    }
}
