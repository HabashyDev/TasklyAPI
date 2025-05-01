using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskly.Core.DTOs
{
    public class RegisterDto
    {
        public string username {  get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}
