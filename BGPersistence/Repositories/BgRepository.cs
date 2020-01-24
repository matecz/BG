using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using bgAPI.Entities.Base;
using BGBLL.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BGPersistence.Repositories
{
    public abstract class BgRepository<T> : IBgRepository<T> where T : EntityBase
    {
        protected DataContext _context;
        
        public BgRepository(DataContext context)
        {
            _context = context;
        }

        public ValueTask<T> GetById(int id) => _context.Set<T>().FindAsync(id);

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
            => _context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();       
        }

        public Task Update(T entity)
        {
            var toUpdate =  GetById(entity.Id).Result;
            if (toUpdate != null)
            {
                _context.Entry(toUpdate).CurrentValues.SetValues(entity);
            }
            return _context.SaveChangesAsync();
        }

        public Task Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public Task<int> CountAll() => _context.Set<T>().CountAsync();

        public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
            => _context.Set<T>().CountAsync(predicate);
    }
}