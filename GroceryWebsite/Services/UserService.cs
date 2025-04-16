using GroceryWebsite.Data;
using GroceryWebsite.DTOs;
using GroceryWebsite.Models;
using GroceryWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GroceryWebsite.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("userId");
            int userId = int.Parse(userIdClaim.Value);
            return userId;
        }

        public User UpdateUser(UpdateUserRequest updateUserRequest)
        {
            int userId = GetUserId();

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

        public async Task ResetPasswordAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                throw new Exception("Email doesn't exist.");

            // Random mật khẩu mới
            var newPassword = GenerateRandomPassword();

            // Cập nhật mật khẩu
            user.Password = _passwordHasher.HashPassword(user, newPassword);
            await _context.SaveChangesAsync();

            // Gửi email
            var subject = "Mật khẩu mới của bạn";
            var body = $"<p>Chúng tôi đã cấp cho bạn một mật khẩu mới như sau:</p>" +
                       $"<h3>{newPassword}</h3>" +
                       $"<p>Hãy đăng nhập và đổi lại mật khẩu.</p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        private string GenerateRandomPassword(int length = 10)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public User ChangePassword(UpdatePasswordRequest updatePasswordRequest)
        { 
            var userId = GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, updatePasswordRequest.OldPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Current password is incorrect");
            }
            if (updatePasswordRequest.NewPassword != updatePasswordRequest.ConfirmPassword)
            {
                throw new Exception("New password and confirmation do not match");
            }

            user.Password = _passwordHasher.HashPassword(user, updatePasswordRequest.NewPassword);

            _context.SaveChanges();
            return user;
        }
    }
}
