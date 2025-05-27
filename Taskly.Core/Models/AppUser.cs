using Microsoft.AspNetCore.Identity;

namespace Taskly.Core.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<TaskTodo> TasksToDo { get; set; }
    }
}
