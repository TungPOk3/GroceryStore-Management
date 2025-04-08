using GroceryWebsite.Data;
using GroceryWebsite.Models;
using Microsoft.AspNetCore.Identity;
using GroceryWebsite.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GroceryWebsite.Services.Interfaces;
using System.Web;

namespace GroceryWebsite.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration config, IEmailService emailService) 
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = config;
            _emailService = emailService;
        }

        public async Task<User> Register(RegisterRequest registerRequest)
        {
            if (_context.Users.Any(u => u.Email == registerRequest.Email))
            {
                throw new Exception("Email already exists");
            }

            var token = Guid.NewGuid().ToString();
            var user = new User
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                FullName = registerRequest.FullName,
                PhoneNumber = registerRequest.PhoneNumber,
                Address = registerRequest.Address,
                EmailVerificationToken = token
            };

            user.Password = _passwordHasher.HashPassword(user, registerRequest.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            var verifyUrl = $"{_configuration["ApiBaseUrl"]}/api/auth/verify-email?token={HttpUtility.UrlEncode(token)}";

            var subject = "Xác thực tài khoản của bạn";
            var body = $"<p>Xin chào {user.FullName},</p>" +
                       $"<p>Vui lòng xác thực tài khoản bằng cách nhấn vào liên kết bên dưới:</p>" +
                       $"<p><a href='{verifyUrl}'>Xác thực email</a></p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return user;
        }

        public string Login(LoginRequest loginRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == loginRequest.UserName);

            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginRequest.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid username or password");
            }

            if (!user.IsEmailConfirmed)
            {
                throw new Exception("Email is not verified. Please check your inbox.");
            }

            var claims = new List<Claim>
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string VerifyEmail(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmailVerificationToken == token);

            if (user == null)
                throw new Exception("Token is invalid or expired!");

            if (user.IsEmailConfirmed)
                return "Email has been previously verified.";

            user.IsEmailConfirmed = true;
            user.EmailVerificationToken = null;
            _context.SaveChanges();

            return "Email verification successful!";
        }


    }
}
