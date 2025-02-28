using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Interfaces;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.GenericImplementation;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Repositories
{
    public class TransactionsRepository : GenaricRepository<Transaction>, ITransactionsRepository
    {
        private readonly DbSet<Transaction> _transactions;
        public TransactionsRepository(ApplicationDbContext context) : base(context)
        {
            _transactions = context.Set<Transaction>();
        }

        public async Task<IEnumerable<Transaction>> GetPurchaseTransactionsAsync()
        {
            var transaction = await _transactions.Where(t => t.Type == TransactionTypeEnum.Purchase.ToString()).ToListAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetSaleTransactionsAsync()
        {
            var transaction = await _transactions.Where(t => t.Type == TransactionTypeEnum.Sale.ToString()).ToListAsync();
            return transaction;
        }
    }
}
