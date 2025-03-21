using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskly.Core.DTOs
{
    public class TaskDTO
    {
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskDeadLine { get; set; }

    }
}
