using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taskly.Core;
using Taskly.Core.DTOs;
using Taskly.Core.Models;
using TasklyAPI.Extensions;

namespace TasklyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenProvider _tokenProvider;
        private readonly SignInManager<AppUser> _signinManger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,
            SignInManager<AppUser> signinManger, TokenProvider tokenprovider)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signinManger = signinManger;
            _tokenProvider = tokenprovider;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usernameExists = await _userManager.Users
                .AnyAsync(u => u.UserName == request.UserName);

            if (usernameExists)
                return BadRequest(new { error = "Username already exists." });

            var emailExists = await _userManager.Users
                .AnyAsync(u => u.Email == request.Email);

            if (emailExists)
                return BadRequest(new { error = "Email already in use." });

            var userToCreate = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(userToCreate, request.Password);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            return StatusCode(201); // 201 Created
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.username);
            Console.WriteLine(user);
            if (user == null)
            {
                return Unauthorized("user");
            }
            var result = await _signinManger.CheckPasswordSignInAsync(user, request.password, false);
            if (result.Succeeded)
            {
                return Ok(_tokenProvider.CreateToken(user));
            }
            return Unauthorized("password");
        }
    }

}
