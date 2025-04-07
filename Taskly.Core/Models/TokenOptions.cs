using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskly.Core.Models
{
    public class TokenOptions
    {
        public string Issuer { get; set; }
        public string Audiance { get; set; }
        public int LifeTime { get; set; }
        public string SigningKey    { get; set; }
    }
}