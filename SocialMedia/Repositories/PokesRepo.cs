using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public class PokesRepo : IPokesRepo
    {
        private readonly ApplicationDbContext _context;

        public PokesRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPoke> GetUserPokeAsync(string sourceUserId, string pokedUserId)
        {
            var userpoke = await _context.Pokes.FindAsync(sourceUserId, pokedUserId);
            return userpoke;
        }

        public async Task<PagedList<PokeDto>> GetUserPokesAsync(PokesParams pokesParams)
        {
            var users = _context.ApplicationUsers.OrderBy(u => u.UserName).AsQueryable();
            var pokes = _context.Pokes.AsQueryable();
            if (pokesParams.Predicate.ToLower() == "poked")
            {
                pokes = pokes.Where(p => p.sourceUserId == pokesParams.UserId);
                users = pokes.Select(p => p.PokedUser);
            }

            if (pokesParams.Predicate.ToLower() == "pokedby")
            {
                pokes = pokes.Where(p => p.pokedUserId == pokesParams.UserId);
                users = pokes.Select(p => p.SourceUser);
            }
            var retunedUsers = users.Select(user => new PokeDto
            {
                UserName = user.UserName,
                PhotoUrl = user.Photos.FirstOrDefault(u => u.IsMainPhoto == true).PhotoUrl

            });
            return PagedList<PokeDto>.Create(retunedUsers, pokesParams.PageNumber, pokesParams.PageSize);

        }

        public async Task<ApplicationUser> GetUserWithPokesAsync(string userId)
        {
            return await _context.ApplicationUsers.Include(u => u.PokedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
