using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Models;
using SocialMedia.Repositories;
using SocialMedia.Services;

namespace SocialMedia.HUBs
{
    public class ChatHub : Hub
    {
        #region fields
        private readonly IMessageRepo _messageRepo;
        private readonly IMapper _mapper;
        private readonly IApplicationUserRepo _userRepo;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly IPresenceTracker _presenceTracker;
        #endregion

        #region Constructors
        public ChatHub(IMessageRepo messageRepo,
                        IMapper mapper,
                        IApplicationUserRepo userRepo,
                        IHubContext<PresenceHub> presenceHub,
                        IPresenceTracker presenceTracker)
        {
            _messageRepo = messageRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _presenceHub = presenceHub;
            _presenceTracker = presenceTracker;
        }
        #endregion

        #region Methods

        public override async Task OnConnectedAsync()
        {
            var curentUserName = Context.User.GetCurrentUserName();
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"];

            var groupName = GetGroupName(curentUserName, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //for database
            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _messageRepo.GetMessagesThreadAsync(curentUserName, otherUser);
            await Clients.Caller.SendAsync("ReceiveMessage", messages);


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto messageDto)
        {
            var userName = Context.User.GetCurrentUserName();
            if (userName == messageDto.ReceiverUserName)
            {
                throw new HubException("You can't Send Message To yourself");
            }
            var sender = await _userRepo.GetUserByNameAsync(userName);
            var receiver = await _userRepo.GetUserByNameAsync(messageDto.ReceiverUserName);
            if (receiver == null) { throw new HubException("No User With This Name"); }
            var message = new Message()
            {
                SenderUser = sender,
                ReceiverUser = receiver,
                Content = messageDto.Content
            };
            _messageRepo.AddMessage(message);

            var groupName = GetGroupName(sender.UserName, receiver.UserName);

            var group = await _messageRepo.GetMessageGroup(groupName);

            if (group.Connections.Any(u => u.ApplicationUserId == receiver.Id))
            {
                message.ReadAt = DateTime.UtcNow;
            }
            else
            {
                var connections = await _presenceTracker.GetConnectionForUser(Context.User.GetCurrentUserID());
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageComes", new { userName = sender.UserName, KnownAs = sender.KnownAs });
                }
            }
            if (await _messageRepo.SaveAllChangesAsync() > 0)
            {
                var Mappedmessage = _mapper.Map<MessageDto>(message);
                await Clients.Group(groupName).SendAsync("NewMessage", Mappedmessage);
            }
            else
                throw new HubException("Failed To Send Message");
        }


        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _messageRepo.GetMessageGroup(groupName);
            var connection = new ApplicationUserConnection()
            {
                ConnectionID = Context.ConnectionId,
                ApplicationUserId = Context.User.GetCurrentUserID(),
            };
            if (group == null)
            {
                group = new Group()
                {
                    Name = groupName
                };
                _messageRepo.AddGroup(group);
            }
            group.Connections.Add(connection);

            if (await _messageRepo.SaveAllChangesAsync() > 0)
                return group;
            else
                throw new HubException($"Failed To join group {groupName}");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageRepo.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(g => g.ConnectionID == Context.ConnectionId);
            _messageRepo.RemoveConnection(connection);
            if (await _messageRepo.SaveAllChangesAsync() > 0)
            {
                return group;
            }
            else
                throw new HubException($"Failed to remove from group{group.Name}");
        }

        private string GetGroupName(string userName, string anotherUserName)
        {
            var groupNameCompare = string.CompareOrdinal(userName, anotherUserName);
            string groupName;
            if (groupNameCompare < 0)
            {
                groupName = $"{userName}-${anotherUserName}";
            }
            else if (groupNameCompare > 0)
            {
                groupName = $"{anotherUserName}-${userName}";
            }
            else
                groupName = string.Empty;
            return groupName;
        }
        #endregion
    }
}
