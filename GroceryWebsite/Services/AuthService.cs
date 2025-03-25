using GroceryWebsite.Data;
using GroceryWebsite.Models;
using Microsoft.AspNetCore.Identity;
using GroceryWebsite.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GroceryWebsite.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration config) 
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = config;
        }

        public User Register(RegisterRequest registerRequest)
        {
            if (_context.Users.Any(u => u.Email == registerRequest.Email))
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                FullName = registerRequest.FullName,
                PhoneNumber = registerRequest.PhoneNumber,
                Address = registerRequest.Address
            };

            user.Password = _passwordHasher.HashPassword(user, registerRequest.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

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

    }
}
