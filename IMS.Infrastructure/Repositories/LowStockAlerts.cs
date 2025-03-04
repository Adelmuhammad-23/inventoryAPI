using IMS.Domain.Entities;
using IMS.Domain.Interfaces;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.GenericImplementation;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Repositories
{
    public class LowStockAlerts : GenaricRepository<LowStockAlert>, ILowStockAlerts
    {
        private readonly DbSet<LowStockAlert> _alerts;
        public LowStockAlerts(ApplicationDbContext dbContext) : base(dbContext)
        {
            _alerts = dbContext.Set<LowStockAlert>();
        }
    }
}
