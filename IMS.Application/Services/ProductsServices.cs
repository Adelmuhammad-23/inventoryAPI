using AutoMapper;
using IMS.Application.DTOs;
using IMS.Domain.Interfaces;

namespace IMS.Application.Services
{
    public class ProductsServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsServices(IProductRepository productRepository,
                                IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDTO> GetProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, c => c.Category);
            var productMapping = _mapper.Map<ProductDTO>(product);

            return productMapping;
        }
        public async Task<List<ProductDTO>> GetProductListAsync()
        {
            var product = await _productRepository.GetAllAsync(c => c.Category);
            var productMapping = _mapper.Map<List<ProductDTO>>(product);

            return productMapping;
        }
    }
}
