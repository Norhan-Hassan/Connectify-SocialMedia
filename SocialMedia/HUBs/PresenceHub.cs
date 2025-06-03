using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Extensions;
using SocialMedia.Models;

namespace SocialMedia.HUBs
{
    [Authorize]
    public class PresenceHub : Hub
    {
        #region fields

        private readonly ApplicationDbContext _context;

        #endregion


        #region Constructor
        public PresenceHub(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public override async Task OnConnectedAsync()
        {
            var userID = Context.User.GetCurrentUserID();

            var connectionID = Context.ConnectionId;

            if (!string.IsNullOrEmpty(userID))
            {
                var online = await _context.UserConnections.AnyAsync(u => u.ConnectionID == connectionID);
                if (online == false)
                {
                    var userconnection = new ApplicationUserConnection()
                    {
                        ConnectionID = connectionID,
                        ApplicationUserId = userID,
                    };
                    _context.UserConnections.Add(userconnection);
                    await _context.SaveChangesAsync();
                }

                await Clients.Others.SendAsync("online", Context.User.GetCurrentUserName());
            }

            await base.OnConnectedAsync();

        }




        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionID = Context.ConnectionId;
            var connectionUser = await _context.UserConnections
                                                 .FirstOrDefaultAsync(c => c.ConnectionID == connectionID);

            if (connectionUser != null)
            {
                _context.UserConnections.Remove(connectionUser);
                await _context.SaveChangesAsync();

                var otherConnections = await _context.UserConnections
                                .Where(c => c.ApplicationUserId == connectionUser.ApplicationUserId)
                                .ToListAsync();

                if (otherConnections.Any() == false)
                {
                    await Clients.Others.SendAsync("offline", Context.User.GetCurrentUserName());
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        #endregion

    }
}
