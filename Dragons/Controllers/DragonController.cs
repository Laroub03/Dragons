using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dragons.Models;
using Dragons.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Dragons.Controllers
{
    // Define DragonController class for handling dragon-related API endpoints
    [Route("api/[controller]")]
    [ApiController]
    public class DragonController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Constructor for DragonController, injecting IAuthService
        public DragonController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST endpoint for registering a dragon
        [HttpPost("register")]
        public IActionResult Register([FromBody] DragonRegistrationModel model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to register the dragon using the injected AuthService
            var dragon = _authService.RegisterDragon(model);

            // Handle registration success or failure
            if (dragon == null)
            {
                return BadRequest(new { message = "Registration failed. Username may already be taken." });
            }

            return Ok(new { message = "Registration successful" });
        }

        // POST endpoint for authenticating a dragon
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] DragonLoginModel model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = model.Username;
            var password = model.Password;

            // Attempt to authenticate the dragon using the injected AuthService
            var dragon = _authService.Authenticate(username, password);

            // Handle authentication success or failure
            if (dragon == null)
            {
                return Unauthorized(new { message = "Authentication failed. Invalid username or password." });
            }

            // Generate a JWT token for the authenticated dragon
            var token = _authService.GenerateJwtToken(username, dragon.IsSongwriter);

            return Ok(new { token });
        }

        // GET endpoint for retrieving dragon's favorite songs
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("dragontunes")]
        public IActionResult GetDragontunes()
        {
            // Get the username of the authenticated dragon
            var username = HttpContext.User.Identity.Name;

            return Ok(new { message = $"Welcome, {username}! Here are your favorite Dragontunes." });
        }
    }
}