using Dragons.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dragons.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Dragons.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IDragonSongService _songService;

        public MusicController(IDragonSongService songService)
        {
            _songService = songService;
        }

        [HttpGet("dragetunes")]
        public IActionResult GetDragonTunes()
        {
            var currentUser = HttpContext.User.Identity.Name; 
            var topTenSongs = _songService.GetTopTenSongs();
            return Ok(topTenSongs);
        }
    }

}
