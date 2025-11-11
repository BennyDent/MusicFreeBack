
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace MusicFree.Models
{
    public class SongViews:ViewClass
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid SongId { get; set; }

        public User user { get; set; }
    
        public Song song { get; set; }

        
     
     
        public SongViews() { }
        public SongViews( User User, Song Song):base()
        {
            last_listened = DateTime.Now;
            Id = Guid.NewGuid();
            UserId = User.Id;
            user = User;
            SongId = Song.Id;
            listened = 1;
            this.song = Song;
            
        }
      

    }
}
