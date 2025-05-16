using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Repositories;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IApplicationUserRepo _userRepo;
        private readonly IMapper _mapper;
        public UserController(IApplicationUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{name:alpha}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _userRepo.GetMemberByNameAsync(name);

            return Ok(user);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(MemberUpdateDto updateDto)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRepo.GetUserByNameAsync(userName);
            _mapper.Map(updateDto, user);

            _userRepo.Update(user);

            if (await _userRepo.SaveAllChangesAsync() > 0)
            {
                return Ok("User Updated Successfully");
            }
            else
            {
                return BadRequest("Failed to update this user");
            }
        }
    }
}
