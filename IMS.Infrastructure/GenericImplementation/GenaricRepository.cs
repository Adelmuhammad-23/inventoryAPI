using IMS.Domain.Generic;
using IMS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.GenericImplementation
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenaricRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<T> AddSync(T obj)
        {
            var entityEntry = await _context.AddAsync(obj);
            return obj;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var element = await GetByIdAsync(id);
            _context.Remove(element);
            return element;
        }

        public async Task<T> UpdatAsync(int id)
        {
            var element = await GetByIdAsync(id);
            _context.Update(element);
            return element;
        }
    }
}
