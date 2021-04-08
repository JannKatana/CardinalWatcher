using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "UserName taken");
                return ValidationProblem();
            }

            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                MiddleInitial = registerDto.MiddleInitial,
                Suffix = registerDto.Suffix,
                Department = registerDto.Department,
                Email = registerDto.Email,
                UserName = registerDto.Username,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem Registering user");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<ActionResult<UserDto>> ResetPassword(LoginDto loginDto)
        {
            var requestingUser = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if (!requestingUser.IsSuperUser) return Unauthorized();

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, "Pa$$w0rd");


            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<UserDto>> ChangePassword(LoginDto loginDto)
        {
            var requestingUser = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();
            if (requestingUser.Email != user.Email) return Unauthorized();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, loginDto.Password);


            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                Email = user.Email,
                DisplayName = $"{user.FirstName} {user.LastName}"
                + (string.IsNullOrEmpty(user.Suffix) ? string.Empty : $" {user.Suffix}"),
                Department = user.Department,
                Image = null,
                IsPresent = user.IsPresent,
                IsSuperUser = user.IsSuperUser
            };
        }
    }
}
