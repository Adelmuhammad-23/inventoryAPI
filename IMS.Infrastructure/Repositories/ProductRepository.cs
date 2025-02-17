using IMS.Domain.Entities;
using IMS.Domain.Interfaces;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.GenericImplementation;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Repositories
{
    public class ProductRepository : GenaricRepository<Product>, IProductRepository
    {
        private readonly DbSet<Product> _products;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _products = context.Set<Product>();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _products.AsNoTracking().Include(c => c.Category).Where(x => x.Id == id).FirstOrDefaultAsync();
            return product;
        }
    }
}
