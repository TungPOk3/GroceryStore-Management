using GroceryWebsite.DTOs;
using GroceryWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryWebsite.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("get-cart")]
        public IActionResult GetCart()
        {
            var cart = _cartService.GetCart();

            if (cart.CartDetails == null || !cart.CartDetails.Any())
            {
                return Ok(new { message = "Giỏ hàng đã được tạo thành công.", cart });
            }

            return Ok(new { message = "Danh sách sản phẩm trong giỏ hàng.", cart.CartDetails });

        }

        [HttpPost("add-to-cart")]
        public IActionResult AddToCart([FromBody] AddToCartRequest addToCartRequest)
        {
            try
            {
                var cartDetail = _cartService.AddToCart(addToCartRequest.ProductId, addToCartRequest.Quantity);
                return Ok(cartDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-cart")]
        public IActionResult UpdateCart([FromBody] UpdateCartRequest updateCartRequest)
        {
            try
            {
                var cartDetail = _cartService.UpdateCart(updateCartRequest.Quantities);
                return Ok(cartDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-from-cart/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            bool isRemoved = _cartService.RemoveFromCart(productId);

            if (isRemoved)
            {
                return Ok(new { message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
            }
            else
            {
                return BadRequest(new { message = "Sản phẩm không tồn tại trong giỏ hàng." });
            }
        }

    }
}
