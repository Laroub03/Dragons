using Dragons.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dragons.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Dragons.Controllers
{
    // Define MusicController class for handling music-related API endpoints
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IDragonSongService _songService;

        // Constructor for MusicController, injecting IDragonSongService
        public MusicController(IDragonSongService songService)
        {
            _songService = songService;
        }

        // GET endpoint for retrieving the top ten dragon songs
        [HttpGet("dragetunes")]
        public IActionResult GetDragonTunes()
        {
            // Get the username of the authenticated dragon
            var currentUser = HttpContext.User.Identity.Name;

            // Retrieve the top ten dragon songs using the injected service
            var topTenSongs = _songService.GetTopTenSongs();

            return Ok(topTenSongs);
        }
    }
}