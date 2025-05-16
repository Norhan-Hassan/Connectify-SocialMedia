using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IApplicationUserRepo _UserRepo;

        public AccountController(IApplicationUserRepo UserRepo)
        {
            _UserRepo = UserRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var result = await _UserRepo.Register(registerDto);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Register), registerDto);
            }

            var error = result.Errors.Select(e => e.Description).ToList();
            var code = UnprocessableEntity().StatusCode;

            return UnprocessableEntity(new { code = code, error = error });

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var result = await _UserRepo.Login(loginRequest);

            if (result == null)
                return Unauthorized("User name or password is invalid");

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _UserRepo.LogOut();

            return Ok("You Logged out successfully");
        }
    }

}
