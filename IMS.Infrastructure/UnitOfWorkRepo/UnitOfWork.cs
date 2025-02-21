using IMS.Domain.Entities;
using IMS.Domain.Generic;
using IMS.Domain.UnitOfWorkInterface;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.GenericImplementation;

namespace IMS.Infrastructure.UnitOfWorkRepo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenaricRepository<Product> Products { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new GenaricRepository<Product>(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
