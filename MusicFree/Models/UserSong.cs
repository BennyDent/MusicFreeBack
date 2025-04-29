using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class UserSong
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SongId { get; set; }
        public string UserId { get; set; }
        public Song Song  { get; set; }
        public UserSong()
        { }
        public UserSong(Guid song_id,string author_id, Song song)
        {
            Id = Guid.NewGuid();
            SongId = song_id;
            UserId = author_id;
            Song = song;
          
        }
    }
}
