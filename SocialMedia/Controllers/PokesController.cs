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

        [HttpPost("{userName:alpha}")]
        public async Task<IActionResult> AddPoke(string userName)
        {
            var sourceUserId = User.GetCurrentUserID();
            var sourceUser = await _pokesRepo.GetUserWithPokesAsync(sourceUserId);

            var pokedUser = await _userRepo.GetUserByNameAsync(userName);

            if (pokedUser == null) { return NotFound("Target User is not found "); }

            if (sourceUser.Id == pokedUser.Id) { return BadRequest("You can't poke yourself"); }

            var userPoke = await _pokesRepo.GetUserPokeAsync(sourceUserId, pokedUser.Id);


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
