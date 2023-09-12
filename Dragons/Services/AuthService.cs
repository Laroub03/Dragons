using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Dragons.Models;

namespace Dragons.Services
{
    // Define IAuthService interface for authentication-related operations
    public interface IAuthService
    {
        Dragon RegisterDragon(DragonRegistrationModel model);
        Dragon Authenticate(string username, string password);
        string GenerateJwtToken(string username, bool isSongwriter);
    }

    // Implementation of IAuthService
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly List<Dragon> _dragonStore;
        private readonly string _secretKey;

        // Constructor for AuthService, injecting IConfiguration
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dragonStore = new List<Dragon>();
            _secretKey = _configuration["AppSettings:SecretKey"];
        }

        // Register a new dragon
        public Dragon RegisterDragon(DragonRegistrationModel model)
        {
            // Check if a dragon with the same username already exists
            if (_dragonStore.Exists(d => d.Username == model.Username))
            {
                return null; // Username is already taken
            }

            // Hash the password securely using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var dragon = new Dragon
            {
                Username = model.Username,
                PasswordHash = hashedPassword,
                IsSongwriter = false, // Set IsSongwriter to false by default during registration
            };

            _dragonStore.Add(dragon);

            return dragon;
        }

        // Authenticate a dragon
        public Dragon Authenticate(string username, string password)
        {
            var dragon = _dragonStore.Find(d => d.Username == username);

            if (dragon != null && BCrypt.Net.BCrypt.Verify(password, dragon.PasswordHash))
            {
                return dragon;
            }

            return null;
        }

        // Generate a JWT token for a dragon
        public string GenerateJwtToken(string username, bool isSongwriter)
        {
            var secretKey = _configuration["AppSettings:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, isSongwriter ? "Songwriter" : "MusicLover"),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
