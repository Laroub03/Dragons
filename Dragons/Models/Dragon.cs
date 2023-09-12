namespace Dragons.Models
{
    public class Dragon
    {
        public int Id { get; set; } // Unique identifier for the dragon

        public string Username { get; set; }

        public string PasswordHash { get; set; } // Store the hashed password
        public bool IsSongwriter { get; set; }
    }
}
