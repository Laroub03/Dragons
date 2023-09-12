using System.ComponentModel.DataAnnotations;

namespace Dragons.Models
{
    public class DragonRegistrationModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
