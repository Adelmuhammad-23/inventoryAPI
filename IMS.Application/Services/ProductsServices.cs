using IMS.Domain.Entities;
using IMS.Domain.Interfaces;

namespace IMS.Application.Services
{
    public class ProductsServices
    {
        private readonly IProductRepository _productRepository;
        public ProductsServices(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            return product;
        }
    }
}
