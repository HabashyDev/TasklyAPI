using Microsoft.AspNetCore.Identity;

namespace Taskly.Core.Models
{
    public class AppUser :IdentityUser
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        
    }
}
