using AutoMapper;
using IMS.Application.DTOs.ProductDTOs;
using IMS.Application.Services;
using IMS.Domain.Entities;
using IMS.Domain.Interfaces;
using IMS.Domain.UnitOfWorkInterface;
using IMS.Tests.TestHelpers;
using Moq;

namespace IMS.Tests.ServicesTests
{
    public class ProductsServicesTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductsServices _productService;

        public ProductsServicesTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

            _productService = new ProductsServices(_mockUnitOfWork.Object, _mockProductRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnProductDTO_WhenProductExists()
        {
            var fakeProduct = FakeProductData.GetFakeOneProduct();
            var fakeProductDTO = FakeProductData.GetFakeOneProductDTO();

            _mockProductRepository
                .Setup(repo => repo.GetByIdAsync(1, It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, object>>>()))
                .ReturnsAsync(fakeProduct);

            _mockMapper
                .Setup(mapper => mapper.Map<ProductDTO>(fakeProduct))
                .Returns(fakeProductDTO);

            var result = await _productService.GetProductAsync(1);

            Assert.NotNull(result);
            Assert.Equal(fakeProductDTO.Id, result.Id);
            Assert.Equal(fakeProductDTO.Name, result.Name);
            Assert.Equal(fakeProductDTO.Price, result.Price);
        }
    }
}
