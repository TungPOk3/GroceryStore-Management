using GroceryWebsite.DTOs;
using GroceryWebsite.Services;
using GroceryWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroceryWebsite.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IConfiguration config)
        {
            _authService = authService;
            _configuration = config;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            try
            {
                var user = _authService.Register(registerRequest);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            try
            {
                var token = _authService.Login(loginRequest);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
