using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek_DAL.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class, new()
    {
        protected DbContext Context { get; }

        public Repository(DbContext context)
        {
            this.Context = context;
        }

        public IEnumerable<T> GetEntities()
        {
            return Context.Set<T>().ToList();
        }

       

        public IEnumerable<T> GetEntities(Expression<Func<T, bool>> conditions,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Context.Set<T>();
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            if (conditions != null)
            {
                query = query.Where(conditions);
            }
            return query.ToList();
        }

        public IEnumerable<T> GetEntities(Expression<Func<T, bool>> conditions)
        {
            return GetEntities(conditions, null).ToList();
        }

        public IEnumerable<T> GetEntities(params Expression<Func<T, object>>[] includes)
        {
            return GetEntities(null, includes).ToList();
        }

        public void AddEntity(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void EditEntity(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void AddOrUpdate(T entity)
        {
            Context.Set<T>().AddOrUpdate(entity);
        }

        public void DeleteEntity(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }
    }
}
