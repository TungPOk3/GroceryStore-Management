using System.ComponentModel.DataAnnotations;

namespace GroceryWebsite.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required, MinLength(6)] 
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

    }
}
