using IMS.Domain.Entities;
using IMS.Domain.Generic;
using IMS.Domain.Interfaces;
using IMS.Domain.UnitOfWorkInterface;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.GenericImplementation;

namespace IMS.Infrastructure.UnitOfWorkRepo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenaricRepository<Product> ProductsUOF { get; private set; }
        public IGenaricRepository<Transaction> TransactionUOF { get; private set; }
        public IGenaricRepository<Category> CategoryUOF { get; private set; }
        IAuthenticationRepository AuthenticationUOF { get; }
        IUserRefreshTokenRepository UserRefreshTokenRepositoryUOF { get; }
        IUserRepository UserRepositoryUOF { get; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductsUOF = new GenaricRepository<Product>(_context);
            TransactionUOF = new GenaricRepository<Transaction>(_context);
            CategoryUOF = new GenaricRepository<Category>(_context);
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
