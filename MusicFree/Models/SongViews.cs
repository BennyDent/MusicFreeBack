
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace MusicFree.Models
{
    public class SongViews
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid SongId { get; set; }

  
    
        public Song song { get; set; }

        

        public DateTime last_listened { get; set; }
        public SongViews() { }
        public SongViews( string userId, Guid songId,User user, Song song, DateTime lastlistened)
        {
            last_listened = lastlistened;
            Id = Guid.NewGuid();
            UserId = userId;
            SongId = songId;
            
            this.song = song;
        }
      

    }
}
