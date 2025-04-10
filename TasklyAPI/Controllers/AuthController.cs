using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskly.Core.Models;
using Taskly.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Taskly.Core;
using Taskly.EF;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace TasklyAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {

        public AppUser user;
        private readonly IUnitOfWork unitOfWork;
        private readonly  JwtOptions jwtOptions;
        


        public AuthController(IUnitOfWork _unitOfWork ,JwtOptions _jwtOptions  )
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
            var HashedPassword = new PasswordHasher<AppUser>()
                .HashPassword(user, request.password);

            var UserToRegister = new AppUser()
            {
                UserName = request.username,
                PasswordHash = HashedPassword
            };
            unitOfWork.AppUsers.Create(UserToRegister);
            unitOfWork.complete();

            return Created();
        }

        [HttpPost("login")]
        public IActionResult Login(AppUserDto request) 
        {
            var UserFromDb = unitOfWork.AppUsers.Find(user => user.UserName == request.username);
            if(UserFromDb == null)
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
                    new(ClaimTypes.NameIdentifier , request.username)

                    })

                };
                var SecurityToken = tokenhandler.CreateToken(tokenDescriptor);
                var accessToken = tokenhandler.WriteToken(SecurityToken);

                return Ok(accessToken);
            }
        }
        
    }
}
