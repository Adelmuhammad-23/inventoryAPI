using IMS.Domain.Entities;
using IMS.Domain.Generic;

namespace IMS.Domain.UnitOfWorkInterface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenaricRepository<Product> ProductsUOF { get; }
        IGenaricRepository<Transaction> TransactionUOF { get; }
        IGenaricRepository<Category> CategoryUOF { get; }

        Task<int> Complete();
    }
}
