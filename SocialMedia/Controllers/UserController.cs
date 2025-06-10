using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Extensions;
using SocialMedia.Helpers;
using SocialMedia.Models;
using SocialMedia.Repositories;
using SocialMedia.Services;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        #region fields
        private readonly IApplicationUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        #endregion

        #region constructor
        public UserController(IApplicationUserRepo userRepo, IMapper mapper, IPhotoService photoService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _photoService = photoService;
        }
        #endregion

        #region endpoints
        [HttpGet("List")]
        public IActionResult GetAllUsers([FromQuery] UserParams userParams)
        {
            var userName = User.GetCurrentUserName();
            userParams.CurrentUserName = userName;
            var users = _userRepo.GetMembers(userParams);
            Response.AddPagenationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

        [HttpGet("{name:alpha}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _userRepo.GetMemberByNameAsync(name);

            return Ok(user);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await _userRepo.GetUserByNameAsync(User.GetCurrentUserName());
            var mappedUser = _mapper.Map<MemberProfileDto>(user);
            return Ok(mappedUser);
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

        [HttpPut("change-main-photo/{photoId:int}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            var userName = User.GetCurrentUserName();
            var user = await _userRepo.GetUserByNameAsync(userName);

            var photo = user.Photos.FirstOrDefault(p => p.ID == photoId);
            if (photo.IsMainPhoto == true)
            {
                return BadRequest("This is already your main photo");
            }
            var currentMainPhoto = user.Photos.FirstOrDefault(p => p.IsMainPhoto == true);
            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMainPhoto = false;
            }
            photo.IsMainPhoto = true;

            if (await _userRepo.SaveAllChangesAsync() > 0)
            {
                return Ok("Main photo is changed");
            }
            return BadRequest("Failed to change main photo");
        }

        [HttpDelete("delete-photo/{photoId:int}")]

        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var userName = User.GetCurrentUserName();
            var user = await _userRepo.GetUserByNameAsync(userName);

            var photo = user.Photos.FirstOrDefault(p => p.ID == photoId);
            if (photo == null)
            {
                return NotFound("No photo with this id");
            }
            if (photo.IsMainPhoto == true)
            {
                return BadRequest("You can't delete your main photo");
            }
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }
                user.Photos.Remove(photo);
                if (await _userRepo.SaveAllChangesAsync() > 0)
                {
                    return Ok("Photo deleted successfully");
                }
            }
            return BadRequest("Failed to delete this photo");
        }
        #endregion
    }
}
