using IMS.Application.DTOs.CategoryDTOs;
using IMS.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryServices _categoryServices;

        public CategoryController(CategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var getCategories = await _categoryServices.GetCategoriesAsync();
            if (getCategories == null)
                return NotFound("Not found Categories yet");
            return Ok(getCategories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync([FromRoute] int id)
        {
            var categoy = await _categoryServices.GetCategoryByIdAsync(id);
            if (categoy == null)
                return NotFound("Not found Categories yet");
            return Ok(categoy);

        }
        [HttpPost]
        public async Task<IActionResult> AddCategoriesAsync(AddCategoryDTO addCategoryDTO)
        {
            var addCategory = await _categoryServices.AddCategoriesAsync(addCategoryDTO);
            if (addCategory == null)
                return NotFound("Not found Categories yet");

            return Ok(addCategory);
        }
        [HttpPost("Products")]
        public async Task<IActionResult> AddProductToCategoryAsync(int categoyId, int productId)
        {
            var addProductToCategoryResult = await _categoryServices.AddProductToCategoryAsync(categoyId, productId);
            if (addProductToCategoryResult == null)
                return NotFound($"Not found Category or Product with ID's: {categoyId} & {productId}");

            return Ok(addProductToCategoryResult);
        }
        [HttpDelete("Products")]
        public async Task<IActionResult> RemoveProductToCategoryAsync(int categoyId, int productId)
        {
            var removeProductToCategoryResult = await _categoryServices.RemoveProductToCategoryAsync(categoyId, productId);
            if (removeProductToCategoryResult == null)
                return NotFound($"Not found Category or Product with ID's: {categoyId} & {productId}");

            return Ok(removeProductToCategoryResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoriesAsync([FromRoute] int id, [FromBody] UpdateCategoryDTO updateCategory)
        {
            var editCategory = await _categoryServices.UpdateCategoriesAsync(id, updateCategory);
            if (editCategory == null)
                return NotFound("Not found Categories yet");
            return Ok(editCategory);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriesAsync(int id)
        {
            var deleteCategory = await _categoryServices.DeleteCategoriesAsync(id);
            if (deleteCategory == null)
                return NotFound("Not found Categories yet");
            return Ok(deleteCategory);

        }
    }
}
