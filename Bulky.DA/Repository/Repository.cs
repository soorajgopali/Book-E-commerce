using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DA.Data;
using Bulky.DA.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DA.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbset;
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
            _db.Products.Include(u => u.Category).Include(u=>u.CategoryId);
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public List<T> ExecuteRawSqlQuery(string sqlQuery, params object[] parameters)
        {
            return _db.Database.SqlQueryRaw<T>(sqlQuery, parameters).ToList();
        }

        public T Get(Expression<Func<T, bool>> predicate, string? includeproperties = null, bool tracked = false)
        {
            if(tracked){
                IQueryable<T> query = dbset;
                query = query.Where(predicate);
                if (!string.IsNullOrEmpty(includeproperties))
                {
                    foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
                }
                return query.FirstOrDefault();
            }
            else
            {
                IQueryable<T> query = dbset.AsNoTracking();
                query = query.Where(predicate);
                if (!string.IsNullOrEmpty(includeproperties))
                {
                    foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
                }
                return query.FirstOrDefault();
            }
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate, string? includeproperties = null)
        {
            IQueryable<T> query = dbset;
            if (predicate != null)
            {
            query = query.Where(predicate);

            }
            if (!string.IsNullOrEmpty(includeproperties))
            {
                foreach (var property in includeproperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            dbset.Update(entity);
        }
    }
}
