using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskly.Core.Enums;

namespace Taskly.Core.DTOs
{
    public class TaskDTO
    {

        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskDeadLine { get; set; } = DateTime.UtcNow.AddDays(1);
        public TaskTodoStaus TaskStatus { get; set; } = TaskTodoStaus.pending;

        

    }
}
