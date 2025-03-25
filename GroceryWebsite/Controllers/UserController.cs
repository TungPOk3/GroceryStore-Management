using GroceryWebsite.DTOs;
using GroceryWebsite.Services;
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
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPut("update-profile")]
        public IActionResult UpdateProfile([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (userId == 0)
                {
                    return Unauthorized(new {message = "Invalid token"});
                }

                var updatedUser = _userService.UpdateUser(userId, updateUserRequest);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
