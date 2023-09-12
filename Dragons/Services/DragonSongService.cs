using Dragons.Models;

namespace Dragons.Services
{
    public interface IDragonSongService
    {
        List<DragonSong> GetTopTenSongs();
    }

    public class DragonSongService : IDragonSongService
    {
        private List<DragonSong> _songs; 

        public DragonSongService()
        {
            _songs = new List<DragonSong>
            {
            new DragonSong { Id = 1, Title = "Dragon's Midnight Flight" },
            new DragonSong { Id = 2, Title = "Whispers of the Dragon" },
            new DragonSong { Id = 3, Title = "Dragonfire Serenade" },
            new DragonSong { Id = 4, Title = "Dragon Dancing under the Stars" },
            new DragonSong { Id = 5, Title = "The Dragon and the Dreamer" },
            new DragonSong { Id = 6, Title = "Echoes of a Dragon's Roar" },
            new DragonSong { Id = 7, Title = "Dragon's Lullaby" },
            new DragonSong { Id = 8, Title = "Sapphire Dragon Skies" },
            new DragonSong { Id = 9, Title = "The Dragon's Secret Melody" },
            new DragonSong { Id = 10, Title = "Realm of the Fire Dragon" }
            };
        }

        public List<DragonSong> GetTopTenSongs()
        {
            return _songs;
        }
    }

}
