using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Taskly.Core.Models;

namespace TasklyAPI.Extensions
{
    public class TokenProvider
    {
        private readonly IConfiguration _configuration;
        public TokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(AppUser user)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtOptions:Issuer"],
                Audience = _configuration["JwtOptions:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SigningKey"]))
                , SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[] {

                    new(ClaimTypes.NameIdentifier, user.Id) 
                }),
                
            }; 
            var SecurityToken = tokenhandler.CreateToken(tokenDescriptor);
            var accessToken = tokenhandler.WriteToken(SecurityToken);

            return accessToken;
        }
        
    }
}
