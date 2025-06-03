using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Helpers;
using SocialMedia.Models;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        #region fields
        private readonly IMessageRepo _messageRepo;
        private readonly IApplicationUserRepo _userRepo;
        private readonly IMapper _mapper;
        #endregion
        #region constructor
        public MessageController(IMessageRepo messageRepo, IApplicationUserRepo userRepo, IMapper mapper)
        {
            _messageRepo = messageRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }
        #endregion

        #region endpoints

        [HttpPost]
        public async Task<IActionResult> AddMessage(CreateMessageDto messageDto)
        {
            var userName = User.GetCurrentUserName();
            if (userName == messageDto.ReceiverUserName)
            {
                return BadRequest("You can't Send Message To yourself");
            }
            var sender = await _userRepo.GetUserByNameAsync(userName);
            var receiver = await _userRepo.GetUserByNameAsync(messageDto.ReceiverUserName);
            if (receiver == null) { return NotFound("No User With This Name"); }
            var message = new Message()
            {
                SenderUser = sender,
                ReceiverUser = receiver,
                Content = messageDto.Content

            };
            _messageRepo.AddMessage(message);
            if (await _messageRepo.SaveAllChangesAsync() > 0)
            {
                return Ok(_mapper.Map<MessageDto>(message));
            }
            return BadRequest("Failed To Send Message");
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageForUser([FromQuery] MessageParams messageParams)
        {
            var userName = User.GetCurrentUserName();
            messageParams.UserName = userName;
            var messages = await _messageRepo.GetMessagesForUserAsync(messageParams);
            Response.AddPagenationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(messages);
        }
        [HttpGet("thread/{userName:alpha}")]
        public async Task<IActionResult> GetMessageThread(string userName)
        {
            var currentUserName = User.GetCurrentUserName();
            return Ok(await _messageRepo.GetMessagesThreadAsync(currentUserName, userName));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var currentUserName = User.GetCurrentUserName();
            var message = await _messageRepo.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound("No message with this id");
            }

            if (message.SenderUser.UserName != currentUserName
              && message.ReceiverUser.UserName != currentUserName)
            {
                return Unauthorized("You are not alowed to do this action");
            }
            //check for the side of deletion
            if (message.SenderUser.UserName == currentUserName)
            {
                message.SenderDeleted = true;
            }
            if (message.ReceiverUser.UserName == currentUserName)
            {
                message.ReceiverDeleted = true;
            }
            if (message.ReceiverDeleted && message.SenderDeleted)
            {
                _messageRepo.DeleteMessage(message);
            }

            if (await _messageRepo.SaveAllChangesAsync() > 0)
            {
                return Ok("Message deleted successfully");
            }
            return BadRequest("Failed To delete this message");

        }
        #endregion
    }
}
