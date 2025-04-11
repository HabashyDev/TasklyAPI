using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Taskly.Core;
using Taskly.Core.DTOs;
using Taskly.Core.Models;

namespace TasklyAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public AppUser user;
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtOptions jwtOptions;



        public AuthController(IUnitOfWork _unitOfWork, JwtOptions _jwtOptions)
        {
            unitOfWork = _unitOfWork;
            jwtOptions = _jwtOptions;
        }

        [HttpGet]
        public IActionResult getAllUsers()
        {
            return Ok(unitOfWork.AppUsers.getAll());
        }

        [HttpPost("Register")]
        public IActionResult Register(AppUserDto request)
        {
            if (unitOfWork.AppUsers.include(U => U.UserName == request.username))
            {
                return BadRequest("username Exist");
            }
            else
            {
                var HashedPassword = new PasswordHasher<AppUser>()
                    .HashPassword(user, request.password);

                var UserToRegister = new AppUser()
                {
                    UserName = request.username,
                    PasswordHash = HashedPassword,
                    Role = request.role,

                };
                unitOfWork.AppUsers.Create(UserToRegister);
                unitOfWork.complete();

                return Created();
            }
        }

        [HttpPost("login")]
        public IActionResult Login(AppUserDto request)
        {
            var UserFromDb = unitOfWork.AppUsers.Find(user => user.UserName == request.username);
            if (UserFromDb == null)
            {
                return BadRequest("Username or Password is Not Correct");
            }
            if (new PasswordHasher<AppUser>()
                 .VerifyHashedPassword(user, UserFromDb.PasswordHash, request.password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Username or Password is Not Correct");
            }
            else
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = jwtOptions.Issuer,
                    Audience = jwtOptions.Audiance,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                    , SecurityAlgorithms.HmacSha256),
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new(ClaimTypes.NameIdentifier , request.username),
                    new(ClaimTypes.Role,request.role)

                    })

                };
                var SecurityToken = tokenhandler.CreateToken(tokenDescriptor);
                var accessToken = tokenhandler.WriteToken(SecurityToken);

                return Ok(accessToken);
            }
        }

    }
}
