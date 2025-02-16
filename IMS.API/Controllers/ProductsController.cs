using IMS.Application.Services;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("GetProductDetails/{id}")]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            var product = await _productServices.GetProductAsync(id);
            return Ok(new { Message = "Get product Details is successfully ", product });
        }

    }
}
