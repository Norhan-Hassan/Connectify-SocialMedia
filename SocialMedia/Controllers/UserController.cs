using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Models;
using SocialMedia.Repositories;
using SocialMedia.Services;

namespace SocialMedia.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IApplicationUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UserController(IApplicationUserRepo userRepo, IMapper mapper, IPhotoService photoService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _photoService = photoService;
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

            var userName = User.GetCurrentUserName();
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

        [HttpPost("addPhoto")]

        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var userName = User.GetCurrentUserName();
            var user = await _userRepo.GetUserByNameAsync(userName);

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) { return BadRequest(result.Error.Message); }

            var photo = new Photo
            {
                PhotoUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMainPhoto = true;
            }
            user.Photos.Add(photo);
            if (await _userRepo.SaveAllChangesAsync() > 0)
            {
                var photodto = _mapper.Map<PhotoDto>(photo);
                return Created("Photo Uploaded Successfully", photodto);
            }
            return BadRequest("Not Able To Add This Photo");
        }
    }
}
