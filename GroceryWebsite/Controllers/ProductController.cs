using GroceryWebsite.DTOs;
using GroceryWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryWebsite.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("add-product")]
        public IActionResult AddProduct([FromBody] CreateProductRequest createProductRequest)
        {
            try
            {
                var product = _productService.AddProduct(createProductRequest);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-product/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] UpdateProductRequest updateProductRequest) 
        {
            try
            {
                var product = _productService.UpdateProduct(id, updateProductRequest);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete-product/{id}")]
        public IActionResult DeleteProduct(int id) 
        {
            try
            {
                var isDeleted = _productService.DeleteProduct(id);
                if (isDeleted)
                {
                    return Ok(new { message = "Product deleted successfully." });
                }
                else
                {
                    return NotFound(new { message = "Product not found or could not be deleted." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
