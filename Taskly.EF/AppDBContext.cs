using Microsoft.EntityFrameworkCore;
using Taskly.Core.Models;

namespace Taskly.EF
{
    public  class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        

        public DbSet<TaskTodo> TasksTodo { get; set; }

        
    }
}
