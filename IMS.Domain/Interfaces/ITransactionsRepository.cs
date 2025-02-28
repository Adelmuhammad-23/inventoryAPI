using IMS.Domain.Entities;
using IMS.Domain.Generic;

namespace IMS.Domain.Interfaces
{
    public interface ITransactionsRepository : IGenaricRepository<Transaction>
    {
        public Task<IEnumerable<Transaction>> GetSaleTransactionsAsync();
        public Task<IEnumerable<Transaction>> GetPurchaseTransactionsAsync();
    }
}
