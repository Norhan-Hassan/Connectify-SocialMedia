using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Extensions;
using SocialMedia.Helpers;
using SocialMedia.Models;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class PokesController : BaseApiController
    {
        #region fields
        private readonly IPokesRepo _pokesRepo;
        private readonly IApplicationUserRepo _userRepo;
        #endregion
        #region constructor
        public PokesController(IPokesRepo pokesRepo, IApplicationUserRepo userRepo)
        {
            _pokesRepo = pokesRepo;
            _userRepo = userRepo;
        }
        #endregion

        #region endpoints

        [HttpPost("{UserName:alpha}")]
        public async Task<IActionResult> AddPoke(string UserName)
        {
            var sourceUserId = User.GetCurrentUserID();
            var pokedUser = await _userRepo.GetUserByNameAsync(UserName);
            var sourceUser = await _pokesRepo.GetUserWithPokesAsync(sourceUserId);
            if (pokedUser == null) { return NotFound("Target User is not found "); }
            if (sourceUser.UserName == UserName) { return BadRequest("You can't poke yourself"); }
            var userPoke = await _pokesRepo.GetUserPokeAsync(sourceUserId, pokedUser.Id);
            //if (userPoke != null) { return BadRequest("You already Poke this user Before"); }

            userPoke = new UserPoke()
            {
                sourceUserId = sourceUserId,
                pokedUserId = pokedUser.Id
            };
            sourceUser.PokedUsers.Add(userPoke);

            if (await _userRepo.SaveAllChangesAsync() > 0)
            {
                return Ok("Your Poke Sent Successfully");
            }

            return BadRequest("Failed To Poke");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPokes([FromQuery] PokesParams pokesParams)
        {
            var currentUserId = User.GetCurrentUserID();
            pokesParams.UserId = currentUserId;
            var users = await _pokesRepo.GetUserPokesAsync(pokesParams);
            Response.AddPagenationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            if (users == null) { return NotFound("No Pokes For This user"); }
            return Ok(users);
        }
        #endregion
    }
}
