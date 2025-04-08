using System.Text.Json.Serialization;

namespace GroceryWebsite.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public bool IsEmailConfirmed { get; set; } = false;
        public string? EmailVerificationToken { get; set; }

        [JsonIgnore]
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();      
    }
}
