using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskly.Core.Models;
using Taskly.Core.Repositories;

namespace Taskly.Core
{
    public interface IUnitOfWork :IDisposable
    {
        IBaseRepository<TaskTodo> TasksToDo { get; }
        void complete();
    }
}
