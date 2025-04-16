using GroceryWebsite.DTOs;
using GroceryWebsite.Services;
using GroceryWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GroceryWebsite.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("update-profile")]
        public IActionResult UpdateProfile([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var updatedUser = _userService.UpdateUser(updateUserRequest);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> QuickResetPassword([FromBody] string email)
        {
            try
            {
                await _userService.ResetPasswordAsync(email);
                return Ok(new { message = "Mật khẩu mới đã được gửi tới email của bạn." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("change-password")]
        public IActionResult ChangePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                _userService.ChangePassword(updatePasswordRequest);
                return Ok(new { message = "Password change successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
