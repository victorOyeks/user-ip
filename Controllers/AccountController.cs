using hngstageone.Dto;
using hngstageone.Entities;
using hngstageone.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace hngstageone.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginDto loginDto)
        {
            var userName = loginDto.Username;
            var password = loginDto.Password;

            if ((userName != "oyeks@email.com") || (password != "Password@1")) return BadRequest("Username or Password is not correct");

            var user = new AppUser
            {
                Email = "oyeks@email.com",
                PasswordHash = "Password@1"
            };

            return Ok(
                new LoginResponse
                {
                    UserName = userName,
                    Token = _tokenService.CreateToken(user)
                });
        }
    }
}
