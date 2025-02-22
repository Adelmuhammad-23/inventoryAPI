using IMS.Application.DTOs.ProductDTOs;
using IMS.Domain.Entities;

namespace IMS.Tests.TestHelpers
{
    public static class FakeProductData
    {
        public static Product GetFakeOneProduct()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "Laptop",
                Price = 1500,
                CreatedAt = DateTime.UtcNow,
                QuantityInStock = 55,
                UpdatedAt = DateTime.UtcNow,
                Description = "test Description",
                Supplier = "Test Supplier"
            };
            return product;
        }
        public static ProductDTO GetFakeOneProductDTO()
        {
            return new ProductDTO
            {
                Id = 1,
                Name = "Laptop",
                Price = 1500,
                CreatedAt = DateTime.UtcNow,
                QuantityInStock = 55,
                UpdatedAt = DateTime.UtcNow,
                Description = "test Description",
                Supplier = "Test Supplier"
            };
        }
        public static List<Product> GetFakeProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1500 ,CategoryId = 1 ,CreatedAt = DateTime.UtcNow,QuantityInStock = 55,UpdatedAt = DateTime.UtcNow,Description = "test Description" ,Supplier ="Test Supplier" },
                new Product { Id = 2, Name = "Mouse", Price = 200 ,CategoryId = 1 ,CreatedAt = DateTime.UtcNow,QuantityInStock = 55,UpdatedAt = DateTime.UtcNow,Description = "test Description" ,Supplier ="Test Supplier" },
                new Product { Id = 3, Name = "Keyboard", Price = 300 ,CategoryId = 1 ,CreatedAt = DateTime.UtcNow,QuantityInStock = 55,UpdatedAt = DateTime.UtcNow,Description = "test Description" ,Supplier ="Test Supplier" },
            };
        }
        public static List<ProductDTO> GetFakeProductDTOList()
        {
            return GetFakeProducts()
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CreatedAt = p.CreatedAt,
                    QuantityInStock = p.QuantityInStock,
                    UpdatedAt = p.UpdatedAt,
                    Description = p.Description,
                    Supplier = p.Supplier
                })
                .ToList();
        }
    }
}
