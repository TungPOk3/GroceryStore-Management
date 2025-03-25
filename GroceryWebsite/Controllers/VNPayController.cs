using GroceryWebsite.Services;
using GroceryWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace GroceryWebsite.Controllers
{
    [ApiController]
    [Route("api/vnpay")]
    public class VNPayController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public VNPayController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [HttpGet("callback")]
        public IActionResult VNPayCallback()
        {
            var vnp_HashSecret = _configuration["VNPay:HashSecret"];
            var vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
            var vnp_TxnRef = Request.Query["vnp_TxnRef"];
            var vnp_SecureHash = Request.Query["vnp_SecureHash"];
            var vnp_Amount = Request.Query["vnp_Amount"];

            // Xác thực chữ ký (bảo mật)
            var vnp_Params = Request.Query
                .Where(kvp => kvp.Key.StartsWith("vnp_") && kvp.Key != "vnp_SecureHash")
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

            string signData = string.Join("&", vnp_Params.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}={kvp.Value}"));

            using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(vnp_HashSecret)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signData));
                string calculatedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                if (calculatedHash != vnp_SecureHash)
                {
                    return BadRequest(new { message = "Invalid signature" });
                }
            }

            int orderId = int.Parse(vnp_TxnRef);
            if (vnp_ResponseCode == "00") // "00" nghĩa là giao dịch thành công
            {
                _orderService.UpdateOrderStatus(orderId, "Paid");
                return Ok(new { message = "Thanh toán thành công!", orderId });
            }
            else
            {
                _orderService.UpdateOrderStatus(orderId, "Failed");
                return BadRequest(new { message = "Thanh toán thất bại!", orderId });
            }
        }
    }
}
