using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.DTOs;
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

        public async Task<IEnumerable<PokeDto>> GetUserPokesAsync(string predicate, string userId)
        {
            var users = _context.ApplicationUsers.OrderBy(u => u.UserName).AsQueryable();
            var pokes = _context.Pokes.AsQueryable();
            if (predicate == "Poked")
            {
                pokes = pokes.Where(p => p.sourceUserId == userId);
                users = pokes.Select(p => p.PokedUser);
            }

            if (predicate == "PokedBy")
            {
                pokes = pokes.Where(p => p.pokedUserId == userId);
                users = pokes.Select(p => p.SourceUser);
            }
            return await users.Select(user => new PokeDto
            {
                UserName = user.UserName,
                PhotoUrl = user.Photos.FirstOrDefault(u => u.IsMainPhoto == true).PhotoUrl

            }).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserWithLikes(string userId)
        {
            return await _context.ApplicationUsers.Include(u => u.PokedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
