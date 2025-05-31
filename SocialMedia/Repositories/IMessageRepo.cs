using SocialMedia.DTOs;
using SocialMedia.Helpers;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public interface IMessageRepo
    {
        Task<Message> GetMessageByIdAsync(int id);
        Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessagesThreadAsync(string currentUserName, string receiverUserName);

        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<int> SaveAllChangesAsync();
    }
}
