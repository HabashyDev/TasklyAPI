
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Taskly.Core.Repositories;

namespace Taskly.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDBContext Context;

        public BaseRepository(AppDBContext _Context)
        {
            Context =_Context;
        }
        public void Create(T TaskTodo)
        {
            Context.Set<T>().Add(TaskTodo);
             
        }

        public T DeleteById(Expression<Func<T, bool>> predicate)
        {
            var itemToDelete  =Context.Set<T>().SingleOrDefault(predicate);
            Context.Set<T>().Remove(itemToDelete);
            
            return (itemToDelete);
        }

        public IEnumerable<T> getAll()
        {
            return (Context.Set<T>().ToArray());
        }

        public T GetById(Expression<Func<T, bool>> predicate)
        {
            return (Context.Set<T>().SingleOrDefault(predicate));
        }

        public T Update(T entity)
        {
            Context.Set<T>().Update(entity);
            return entity;
        }

        
    }
}
