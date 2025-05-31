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
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MessageRepo(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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
            var messages = await _context.Messages.FindAsync(id);
            if (messages == null) return null;
            return messages;
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesThreadAsync(string currentUserName, string receiverUserName)
        {
            var messages = await _context.Messages
                .Include(u => u.SenderUser).ThenInclude(u => u.Photos)
                .Include(u => u.ReceiverUser).ThenInclude(u => u.Photos)
                .Where(
                        m => m.SenderUser.UserName == currentUserName && m.ReceiverUser.UserName == receiverUserName
                        ||
                        m.ReceiverUser.UserName == currentUserName && m.SenderUser.UserName == receiverUserName
                ).OrderBy(m => m.SentAt).ToListAsync();

            var unreadMessages = messages.Where(m => m.ReadAt == null && m.ReceiverUser.UserName == currentUserName).ToList();
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.ReadAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
            var mappedResult = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return mappedResult;
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(m => m.SentAt).AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.ReceiverUser.UserName == messageParams.UserName),
                "Outbox" => query.Where(u => u.SenderUser.UserName == messageParams.UserName),
                _ => query.Where(u => u.ReceiverUser.UserName == messageParams.UserName && u.ReadAt == null)
            };
            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return PagedList<MessageDto>.Create(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
