using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Data;
using SocialMedia.DTOs;
using SocialMedia.Models;
using SocialMedia.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IApplicationUserRepo _UserRepo;

        public AccountController(IApplicationUserRepo UserRepo)
        {
            _UserRepo=UserRepo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result= await _UserRepo.Register(registerDto);

            if(result.Succeeded)
            {
                return CreatedAtAction(nameof(Register), registerDto);
            }
            
            return BadRequest(result.Errors.Select(e => e.Description).ToList());

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            if (!ModelState.IsValid)
                 return BadRequest(ModelState);
            
            var result = await _UserRepo.Login(loginRequest);

            if (result == null)
                return Unauthorized("User name or password is invalid");

            return Ok(result);
        }
    }
    
}
