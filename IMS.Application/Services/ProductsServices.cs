using AutoMapper;
using IMS.Application.DTOs.ProductDTOs;
using IMS.Domain.Entities;
using IMS.Domain.Interfaces;
using IMS.Domain.UnitOfWorkInterface;

namespace IMS.Application.Services
{
    public class ProductsServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsServices(IUnitOfWork unitOfWork,
                                IProductRepository productRepository,
                                IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDTO> GetProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, c => c.Category);
            var productMapping = _mapper.Map<ProductDTO>(product);

            return productMapping;
        }



        public async Task<List<ProductDTO>> GetProductListAsync()
        {
            var product = await _unitOfWork.Products.GetAllAsync(c => c.Category);
            var productMapping = _mapper.Map<List<ProductDTO>>(product);

            return productMapping;
        }
        public async Task<ProductDTO> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.DeleteAsync(id);
            if (product == null)
                return null;
            await _unitOfWork.Complete();
            var productMapping = _mapper.Map<ProductDTO>(product);
            return productMapping;
        }
        public async Task<AddProductDTO> AddProductAsync(AddProductDTO productDTO)
        {
            var productMapping = _mapper.Map<Product>(productDTO);

            var product = await _unitOfWork.Products.AddAsync(productMapping);
            await _unitOfWork.Complete();

            return productDTO;
        }
        public async Task<UpdateProductDTO> UpdateProductAsync(UpdateProductDTO productDTO)
        {

            var productMapping = _mapper.Map<Product>(productDTO);

            var productResult = await _unitOfWork.Products.UpdatAsync(productMapping);
            await _unitOfWork.Complete();

            return productDTO;
        }
    }
}
