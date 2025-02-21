using IMS.Application.DTOs.ProductDTOs;
using IMS.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsServices _productServices;
        public ProductsController(ProductsServices productServices)
        {
            _productServices = productServices;
        }

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
