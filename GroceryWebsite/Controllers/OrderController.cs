using GroceryWebsite.Services;
using GroceryWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryWebsite.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IVNPayService _vnpayService;

        public OrderController(IOrderService orderService, IVNPayService vnpayService)
        {
            _orderService = orderService;
            _vnpayService = vnpayService;
        }

        [HttpPost("payment")]
        public IActionResult CreatePayment([FromBody] int shippingMethod)
        {
            var order = _orderService.GetOrder(shippingMethod);

            if (order == null)
            {
                return Ok(new { message = "Không có sản phẩm nào trong giỏ hàng." });
            }

            string paymentUrl = _vnpayService.CreatePaymentUrl(order.OrderId, order.TotalAmount);

            return Ok(new
            {
                message = "Đơn hàng của bạn.",
                order,
                paymentUrl
            });
        }

        [HttpGet("get-list-orders")]
        public IActionResult GetListOrders()
        {
            var orders = _orderService.GetListOrders();

            if (orders == null || !orders.Any())
            {
                return Ok(new { message = "Không có đơn hàng nào." });
            }

            return Ok(new { message = "Danh sách đơn hàng.", orders });
        }



    }
}
