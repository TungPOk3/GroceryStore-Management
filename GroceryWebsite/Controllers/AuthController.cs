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
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var user = await _authService.Register(registerRequest);
                return Ok(new
                {
                    message = "Register succesfully. Please check your box!",
                    user = new
                    {
                        user.UserId,
                        user.Email,
                        user.FullName
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("verify-email")]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            try
            {
                var result = _authService.VerifyEmail(token);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
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
