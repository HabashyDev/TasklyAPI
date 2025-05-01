using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Taskly.Core.DTOs;
using Taskly.Core.Models;

namespace TasklyAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        //public AppUser user;
        //private readonly IUnitOfWork unitOfWork;

        private readonly UserManager<AppUser> _userManger;
        private readonly JwtOptions _jwtOptions;
        private readonly SignInManager<AppUser> _signInManager;


        public AuthController(JwtOptions jwtOptions, UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManger)
        {
            _jwtOptions = jwtOptions;
            _userManger = userManager;
            _signInManager = signInManger;
        }


        [HttpGet("GetAllUsers")]
        public IActionResult getAllUsers()
        {
            return Ok(_userManger.Users.ToArray());
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {


            if (_userManger.Users.Any(U => U.UserName == request.username))
            {
                return BadRequest("username Exist");
            }

            var userToCreate = new AppUser
            {
                UserName = request.username,
                Email = request.email
            };

            var result = await _userManger.CreateAsync(userToCreate, request.password);
            if (result.Succeeded)
            {
                return Created();
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return BadRequest(ModelState);




        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {

            var user = await _userManger.FindByNameAsync(request.username);
            Console.WriteLine(user);
            if (user == null)
            {
                return Unauthorized("user");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.password, false);
            if (result.Succeeded)
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _jwtOptions.Issuer,
                    Audience = _jwtOptions.Audiance,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey))
                    , SecurityAlgorithms.HmacSha256),
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new(ClaimTypes.NameIdentifier, request.username),
                    new Claim("AppUserId", user.Id) })
                };
                var SecurityToken = tokenhandler.CreateToken(tokenDescriptor);
                var accessToken = tokenhandler.WriteToken(SecurityToken);

                return Ok(accessToken);
            }
            return Unauthorized("password");
        }

    }
}
