using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.Entities;
using UserService.Repositories;
using UserService.Models;

namespace UserService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<(bool Success, string? Error)> RegisterAsync(LoginModal req)
        {
            var existingUser = await _repo.GetByUsernameAsync(req.Email);
            if (existingUser != null)
            {
                return (false, "Username already exists");
            }

            var existingEmail = await _repo.GetByEmailAsync(req.Email);
            if (existingEmail != null)
            {
                return (false, "Email already registered");
            }

            // Create password hash and salt
            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create new user
            var user = new User
            {   Name = req.Name,          
                Email = req.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };

            // Save user
            await _repo.AddAsync(user);

            return (true, null);
        }

        public async Task<(bool Success, string? Token, string? Error)> LoginAsync(string username, string password)
        {
            // Try to find user by username first, then by email
            var user = await _repo.GetByUsernameAsync(username);
            if (user == null)
            {
                user = await _repo.GetByEmailAsync(username);
            }

            if (user == null)
            {
                return (false, null, "Invalid username or password");
            }

            // Verify password
            if (user.PasswordHash == null || user.PasswordSalt == null)
            {
                return (false, null, "Invalid username or password");
            }

            var isValidPassword = VerifyPasswordHash(
                password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!isValidPassword)
            {
                return (false, null, "Invalid username or password");
            }

            // Generate JWT token
            var token = GenerateToken(user);

            return (true, token, null);
        }

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (computed.Length != storedHash.Length) return false;
            for (int i = 0; i < computed.Length; i++)
                if (computed[i] != storedHash[i])
                    return false;
            return true;
        }

        private string GenerateToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key not configured");
            //var issuer = jwtSection.GetValue<string>("Issuer") ?? "UserService";

            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
