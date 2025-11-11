using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class UserSong
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SongId { get; set; }
        public User user { get; set; }
        public string UserId { get; set; }
        public Song song  { get; set; }
        public UserSong()
        { }
        public UserSong( User User, Song Song)
        {
            Id = Guid.NewGuid();
            SongId = song.Id;
            UserId = User.Id;
            song = Song;
            user = User;
        }
    }
}
