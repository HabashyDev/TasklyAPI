using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskly.Core;
using Taskly.Core.Models;
using Taskly.Core.Repositories;
using Taskly.EF.Repositories;

namespace Taskly.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _Context;
        public UnitOfWork(AppDBContext Context)
        {
            _Context = Context;

            TasksToDo = new BaseRepository<TaskTodo>(_Context);
        }

        public IBaseRepository<TaskTodo> TasksToDo { get; private set; }

        public void complete()
        {
             _Context.SaveChanges();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}
