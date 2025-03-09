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
            var getCategories = await _categoryServices.GetCategoryByIdAsync(id);
            if (getCategories == null)
                return NotFound("Not found Categories yet");
            return Ok(getCategories);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategoriesAsync(AddCategoryDTO addCategoryDTO)
        {
            var getCategories = await _categoryServices.AddCategoriesAsync(addCategoryDTO);
            if (getCategories == null)
                return NotFound("Not found Categories yet");
            return Ok(getCategories);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoriesAsync([FromRoute] int id, [FromBody] UpdateCategoryDTO updateCategory)
        {
            var getCategories = await _categoryServices.UpdateCategoriesAsync(id, updateCategory);
            if (getCategories == null)
                return NotFound("Not found Categories yet");
            return Ok(getCategories);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriesAsync(int id)
        {
            var getCategories = await _categoryServices.DeleteCategoriesAsync(id);
            if (getCategories == null)
                return NotFound("Not found Categories yet");
            return Ok(getCategories);
        }
    }
}
