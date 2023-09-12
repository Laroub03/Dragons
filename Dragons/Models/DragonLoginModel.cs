using System.ComponentModel.DataAnnotations;

namespace Dragons.Models
{
    public class DragonLoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
