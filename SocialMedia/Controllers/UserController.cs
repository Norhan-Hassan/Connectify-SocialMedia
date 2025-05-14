using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
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

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() 
        {       
            var users= await _userRepo.GetUsersAsync();
            var usersDto= _mapper.Map<IEnumerable<MemberDto>>(users);

            return Ok(usersDto);
        
        }

       

    }
}
