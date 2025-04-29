using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class MusicianUser
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid AuthorId { get; set; }
        public string UsersId { get; set; }
        public Musician author { get; set; }
       
        public MusicianUser()
        { }
        public MusicianUser(string song_id, Guid author_id, Musician song)
        {
            Id= Guid.NewGuid();
            UsersId = song_id;
            AuthorId = author_id;
            author = song;
          
        }
    }
}
