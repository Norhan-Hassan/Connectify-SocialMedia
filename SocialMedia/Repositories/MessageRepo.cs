using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public class MessageRepo : IMessageRepo
    {
        #region fields
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region constructor
        public MessageRepo(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _context.Messages.Include(u => u.SenderUser).Include(u => u.ReceiverUser)
               .SingleOrDefaultAsync(m => m.ID == id);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesThreadAsync(string currentUserName, string receiverUserName)
        {
            var messages = await _context.Messages
                .Include(u => u.SenderUser).ThenInclude(u => u.Photos)
                .Include(u => u.ReceiverUser).ThenInclude(u => u.Photos)
                .Where(
                        m => m.SenderUser.UserName == currentUserName && m.ReceiverUser.UserName == receiverUserName && m.ReceiverDeleted == false
                        ||
                        m.ReceiverUser.UserName == currentUserName && m.SenderUser.UserName == receiverUserName && m.SenderDeleted == false
                ).OrderBy(m => m.SentAt)
                .ToListAsync();

            var unreadMessages = messages.Where(m => m.ReadAt == null && m.ReceiverUser.UserName == currentUserName).ToList();
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.ReadAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
            var mappedMessages = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return mappedMessages;
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var messages = _context.Messages
                            .OrderByDescending(m => m.SentAt)
                            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                            .AsQueryable();
            messages = messageParams.Container switch
            {
                "Inbox" => messages.Where(u => u.receiverUserName == messageParams.UserName && u.ReceiverDeleted == false),
                "Outbox" => messages.Where(u => u.senderUserName == messageParams.UserName && u.SenderDeleted == false),
                _ => messages.Where(u => u.receiverUserName == messageParams.UserName && u.ReceiverDeleted == false && u.ReadAt == null)
            };


            return PagedList<MessageDto>.Create(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUserConnection> GetUserConnectionAsync(string connectionID)
        {
            return await _context.UserConnections.FindAsync(connectionID);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups.Include(g => g.Connections).FirstOrDefaultAsync(g => g.Name == groupName);
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void RemoveConnection(ApplicationUserConnection connection)
        {
            _context.UserConnections.Remove(connection);
        }

        public async Task<Group> GetGroupForConnection(string connectionID)
        {
            return await _context.Groups.Include(g => g.Connections)
                                        .Where(g => g.Connections.Any(c => c.ConnectionID == connectionID))
                                        .FirstOrDefaultAsync();
        }
        #endregion
    }
}
