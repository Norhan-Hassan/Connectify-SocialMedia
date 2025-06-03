using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Models;

namespace SocialMedia.Services
{
    public class PresenceTracker : IPresenceTracker
    {
        private readonly ApplicationDbContext _context;

        public PresenceTracker(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<string>> GetConnectionForUser(string userId)
        {
            var connectionIDS = await _context.UserConnections
                .Where(c => c.ApplicationUserId == userId)
                .Select(c => c.ConnectionID)
                .ToListAsync();

            return connectionIDS;
        }

        public async Task<List<ApplicationUser>> GetOnlineUsersAsync()
        {

            var onlineUserIds = await _context.UserConnections
                                        .Select(c => c.ApplicationUserId)
                                        .Distinct()
                                        .ToListAsync();

            var onlineUsers = await _context.ApplicationUsers
                                            .Where(u => onlineUserIds.Contains(u.Id))
                                            .ToListAsync();

            return onlineUsers;
        }
    }
}
