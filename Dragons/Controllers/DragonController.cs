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
    [Route("api/[controller]")]
    [ApiController]
    public class DragonController : ControllerBase
    {
        private readonly IAuthService _authService;

        public DragonController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] DragonRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dragon = _authService.RegisterDragon(model);

            if (dragon == null)
            {
                return BadRequest(new { message = "Registration failed. Username may already be taken." });
            }

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] DragonLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = model.Username;
            var password = model.Password;

            var dragon = _authService.Authenticate(username, password);

            if (dragon == null)
            {
                return Unauthorized(new { message = "Authentication failed. Invalid username or password." });
            }

            var token = _authService.GenerateJwtToken(username, dragon.IsSongwriter);

            return Ok(new { token });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("dragontunes")]
        public IActionResult GetDragontunes()
        {
            // Your logic to retrieve and return dragon's music here
            var username = HttpContext.User.Identity.Name;

            return Ok(new { message = $"Welcome, {username}! Here are your favorite Dragontunes." });
        }
    }
}
