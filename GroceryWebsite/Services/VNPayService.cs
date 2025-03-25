using GroceryWebsite.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GroceryWebsite.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(int orderId, decimal amount)
        {
            var vnp_TmnCode = _configuration["VNPay:TmnCode"];
            var vnp_HashSecret = _configuration["VNPay:HashSecret"];
            var vnp_Url = _configuration["VNPay:Url"];
            var vnp_ReturnUrl = _configuration["VNPay:ReturnUrl"];

            var vnp_TxnRef = orderId.ToString();
            var vnp_CreateDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var bankCode = "NCB";
            var vnp_IpAddr = "127.0.0.1";

            var vnp_Params = new SortedDictionary<string, string>
            {
                { "vnp_Amount", ((int)(amount * 100)).ToString() },
                { "vnp_BankCode", bankCode },
                { "vnp_Command", "pay" },
                { "vnp_CreateDate", vnp_CreateDate },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", vnp_IpAddr },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", "Thanh toan don hang" },
                { "vnp_OrderType", "billpayment" },
                { "vnp_ReturnUrl", vnp_ReturnUrl },
                { "vnp_TxnRef", vnp_TxnRef },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Version", "2.1.0" }
            };

            // Tạo chuỗi querystring
            var queryString = new StringBuilder();
            foreach (var item in vnp_Params)
            {
                if (queryString.Length > 0) queryString.Append("&");
                queryString.Append($"{item.Key}={item.Value}");
            }

            // Tạo chữ ký
            var hash = ComputeHmacSHA512(vnp_HashSecret, queryString.ToString());
            queryString.Append($"&vnp_SecureHash={hash}");

            return $"{vnp_Url}?{queryString}";
        }

        private static string ComputeHmacSHA512(string key, string data)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
    
    }
}
