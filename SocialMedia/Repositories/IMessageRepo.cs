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
        Task<ApplicationUserConnection> GetUserConnectionAsync(string connectionID);
        Task<Group> GetGroupForConnection(string connectionID);
        Task<Group> GetMessageGroup(string groupName);
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        void AddGroup(Group group);
        void RemoveConnection(ApplicationUserConnection connection);
        Task<int> SaveAllChangesAsync();
    }
}
