using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DA.Repository.IRepository
{
  public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate=null, string? includeproperties = null);
        T Get(Expression<Func<T, bool>> predicate, string? includeproperties = null, bool tracked=false);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
