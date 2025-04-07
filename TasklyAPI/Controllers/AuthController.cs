using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskly.Core.Models;
using Taskly.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Taskly.Core;

namespace TasklyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUnitOfWork UnitOfWork) : ControllerBase
    {
        [HttpPost]
        public IActionResult Register(AppUserDto request)
        {
            var PasswordHasher = new PasswordHasher<AppUser>();
            var UserToRegister = new AppUser()
            {
                UserName = request.username,
                PasswordHash = request.password,
            };
            UnitOfWork.AppUsers.Create(UserToRegister);
            UnitOfWork.complete();

            return Created();


        }
    }
}
