using IMS.Domain.Generic;
using IMS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IMS.Infrastructure.GenericImplementation
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;
        public GenaricRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync(
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<T> AddAsync(T obj)
        {
            var entityEntry = await _context.Set<T>().AddAsync(obj);
            return obj;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var element = await GetByIdAsync(id);
            _context.Set<T>().Remove(element);
            return element;
        }

        public async Task<T> UpdatAsync(T obj)
        {
            _context.Set<T>().Update(obj);
            return obj;
        }
        public IQueryable<T> GetTableNoTracking()
        {
            return _context.Set<T>().AsNoTracking().AsQueryable();
        }
        public IQueryable<T> GetTableAsTracking()
        {
            return _context.Set<T>().AsQueryable();

        }
    }
}
