using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class Playlist
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }    
        public IList<PlaylistSong> songs {get; } = new List<PlaylistSong>();
       

        public User author { get; set; }

        public DateTime timestamp { get; set; }
        public Playlist()
        {
        }

        public Playlist(string name,  User User )
        {
            author= User;
            Name = name;    
            
            songs = new List<PlaylistSong>();
            timestamp = DateTime.Now;   
        }
    }
}
