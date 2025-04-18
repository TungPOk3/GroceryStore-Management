﻿using System.ComponentModel.DataAnnotations;

namespace GroceryWebsite.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
