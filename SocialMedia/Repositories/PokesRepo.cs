using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
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

        //public Task<IEnumerable<PokeDto>> GetUserPokesAsync(string predicate, string userId)
        //{
        //    var users = _context.ApplicationUsers.OrderBy(u => u.UserName).AsQueryable();
        //    var pokes = _context.Pokes.AsQueryable();

        //}

        public async Task<ApplicationUser> GetUserWithLikes(string userId)
        {
            return await _context.ApplicationUsers.Include(u => u.PokedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
