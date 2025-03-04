using IMS.Application.DTOs.ProductDTOs;
using IMS.Application.Services;
using IMS.Domain.Entities;
using IMS.Infrastructure.DbContext;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly ProductsServices _productServices;
        public ProductsController(ApplicationDbContext context, ProductsServices productServices)
        {
            _productServices = productServices;
            _context = context;
        }
        //[Authorize(Roles = "User")]

        [HttpGet()]
        [SwaggerOperation(
            Summary = "Get product list",
            Description = "This endpoint is used to Get all products. "
        )]
        [SwaggerResponse(200, "Get product Details is successfully", typeof(ProductDTO))]
        [SwaggerResponse(404, "Product not found", typeof(string))]
        public async Task<IActionResult> GetProductList()
        {
            var products = await _productServices.GetProductListAsync();
            if (products == null) return NotFound("No Product is found");
            return Ok(new { Message = "Get all products is successfully ", products });
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get product Details",
            Description = "This endpoint is used to Get product Details. "
        )]
        [SwaggerResponse(200, "Get product Details is successfully", typeof(ProductDTO))]
        [SwaggerResponse(404, "Product not found", typeof(string))]
        public async Task<IActionResult> GetProductDetails([FromRoute] int id)
        {
            var product = await _productServices.GetProductAsync(id);
            if (product == null) return NotFound("This product is not found");
            return Ok(new { Message = "Get product Details is successfully ", product });
        }
        [Authorize(Roles = "Admin,Manager")]

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid CSV file.");

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                bool isHeader = true;
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (isHeader) { isHeader = false; continue; }

                    var values = line.Split(',');

                    if (values.Length < 8) continue;

                    var product = new Product
                    {
                        Name = values[0],
                        Description = values[1],
                        Price = decimal.Parse(values[2]),
                        QuantityInStock = int.Parse(values[3]),
                        Supplier = values[4],
                        CreatedAt = DateTime.Parse(values[5]),
                        UpdatedAt = DateTime.Parse(values[6]),
                        CategoryId = int.Parse(values[7])
                    };

                    _context.Products.Add(product);
                }
                await _context.SaveChangesAsync();
            }

            return Ok("Products imported successfully!");
        }
        [Authorize(Roles = "Admin,Manager")]

        [HttpGet("export-csv")]
        public IActionResult ExportCSV()
        {
            var products = _context.Products.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Name,Description,Price,QuantityInStock,Supplier,CreatedAt,UpdatedAt,CategoryId");

            foreach (var product in products)
            {
                csv.AppendLine($"{product.Name},{product.Description},{product.Price},{product.QuantityInStock},{product.Supplier},{product.CreatedAt},{product.UpdatedAt},{product.CategoryId}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "Products.csv");
        }



        [Authorize(Roles = "Admin,Manager")]
        [HttpPost()]
        [SwaggerOperation(
            Summary = "Delete product ",
            Description = "This endpoint is used delete product. "
        )]
        [SwaggerResponse(200, "Delete product is successfully", typeof(ProductDTO))]
        [SwaggerResponse(404, "Product not found", typeof(string))]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO productDTO)
        {
            var product = await _productServices.AddProductAsync(productDTO);
            if (product == null) return NotFound("No Product is found");
            return Ok(new { Message = "Add this product is successfully ", product });
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Delete product ",
            Description = "This endpoint is used delete product. "
        )]
        [SwaggerResponse(200, "Delete product is successfully", typeof(UpdateProductDTO))]
        [SwaggerResponse(404, "Product not found", typeof(string))]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductDTO productDTO)
        {
            var product = await _productServices.GetProductAsync(id);

            if (product == null) return NotFound("This product is not found");

            var productUpdated = await _productServices.UpdateProductAsync(productDTO);

            return Ok(new { Message = "Update this product is successfully ", productUpdated });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete product ",
            Description = "This endpoint is used delete product. "
        )]
        [SwaggerResponse(200, "Delete product is successfully", typeof(ProductDTO))]
        [SwaggerResponse(404, "Product not found", typeof(string))]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _productServices.DeleteProductAsync(id);
            if (product == null) return NotFound("No Product is found");
            return Ok(new { Message = "Delete this product is successfully ", product });
        }
    }
}
