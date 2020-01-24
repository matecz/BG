using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using bgAPI.Entities.Base;

namespace BGBLL.Interfaces.Persistence
{
    public interface IBgRepository<T> where T : EntityBase
    {
        ValueTask<T> GetById(int id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);

    }
}