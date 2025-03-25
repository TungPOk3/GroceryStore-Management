using GroceryWebsite.Data;
using GroceryWebsite.DTOs;
using GroceryWebsite.Models;
using GroceryWebsite.Services.Interfaces;

namespace GroceryWebsite.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("userId");
            int userId = int.Parse(userIdClaim.Value);
            return userId;
        }

        public User UpdateUser(int userId, UpdateUserRequest updateUserRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FullName = updateUserRequest.FullName;
            user.PhoneNumber = updateUserRequest.PhoneNumber;
            user.Address = updateUserRequest.Address;  

            _context.SaveChanges();

            return user;
        }
    }
}
