using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taskly.Core.Enums;

namespace Taskly.Core.Models
{
    public class TaskTodo
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("AppUser")]

        public string AppUserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskTodoStaus Status { get; set; } = TaskTodoStaus.pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeadLine { get; set; } = DateTime.UtcNow.AddDays(5);
        public AppUser AppUser { get; set; }


    }
}
