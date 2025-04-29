using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class Playlist
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }    
        public IList<PlaylistSong> songs {get; } = new List<PlaylistSong>();
        public string AuthorId { get; set; }

        public DateTime timestamp { get; set; }
        public Playlist()
        {
        }

        public Playlist(string name, string authorId )
        {
            Name = name;    
            AuthorId = authorId;    
            songs = new List<PlaylistSong>();
            timestamp = DateTime.Now;   
        }
    }
}
