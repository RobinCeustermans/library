using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL.Repositories
{
    public interface IRepository<T>
        where T : class, new()
    {
        IEnumerable<T> GetEntities();
        IEnumerable<T> GetEntities(Expression<Func<T, bool>> conditions);
        IEnumerable<T> GetEntities(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetEntities(Expression<Func<T, bool>> conditions,
            params Expression<Func<T, object>>[] includes);
        void AddEntity(T entity);
        void EditEntity(T entity);
        void AddOrUpdate(T entity);
        void DeleteEntity(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
