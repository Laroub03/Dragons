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
    public interface IAuthService
    {
        Dragon RegisterDragon(DragonRegistrationModel model);
        Dragon Authenticate(string username, string password);
        string GenerateJwtToken(string username, bool isSongwriter);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly List<Dragon> _dragonStore; // In-memory data store

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dragonStore = new List<Dragon>();
        }

        public Dragon RegisterDragon(DragonRegistrationModel model)
        {
            // Check if a dragon with the same username already exists
            if (_dragonStore.Exists(d => d.Username == model.Username))
            {
                return null;
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




        public Dragon Authenticate(string username, string password)
        {
            // Implement authentication logic here
            var dragon = _dragonStore.Find(d => d.Username == username);

            if (dragon != null && BCrypt.Net.BCrypt.Verify(password, dragon.PasswordHash))
            {
                return dragon;
            }

            return null;
        }

        public string GenerateJwtToken(string username, bool isSongwriter)
        {
            var secretKey = _configuration["AppSettings:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, isSongwriter ? "Songwriter" : "MusicLover"),
                // Add additional claims here as needed
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Adjust the expiration time as needed
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
