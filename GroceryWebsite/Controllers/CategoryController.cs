using GroceryWebsite.DTOs;
using GroceryWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryWebsite.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("add-category")]
        public IActionResult AddCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            try
            {
                var category = _categoryService.AddCategory(createCategoryRequest);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-category/{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest) 
        {
            try
            {
                var updateCategory = _categoryService.UpdateCategory(id, updateCategoryRequest);
                return Ok(updateCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete-category/{id}")]
        public IActionResult DeleteCategory(int id) 
        {
            try
            {
                var isDeleted = _categoryService.DeleteCategory(id);
                if (isDeleted)
                {
                    return Ok(new { message = "Category deleted successfully." });
                }
                else
                {
                    return NotFound(new { message = "Category not found or could not be deleted." });
                }
            }
            catch
            {
                return BadRequest(new { message = "An error occurred while deleting the category." });
            }
        }

    }
}
