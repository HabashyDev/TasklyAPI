﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskly.Core.DTOs
{
    public class RegisterDto
    {
        public string UserName {  get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
