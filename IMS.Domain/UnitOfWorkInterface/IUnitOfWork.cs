using IMS.Domain.Entities;
using IMS.Domain.Generic;

namespace IMS.Domain.UnitOfWorkInterface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenaricRepository<Product> Products { get; }
        int Complete();
    }
}
