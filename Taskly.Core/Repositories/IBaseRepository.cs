using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Taskly.Core.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> getAll();
        void Create(T entity);
        T Update(T entity);
        T DeleteById(Expression<Func<T, bool>> predicate);
        T GetById(Expression<Func<T, bool>> predicate);
        T Find(Expression<Func<T, bool>> predicate);
        bool include (Expression<Func<T, bool>> predicate);
    }
}
