using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class UserMusician
    {
        [Key]
        public Guid Id { get; set; }
        public User user { get; set; }
        public Guid AuthorId { get; set; }
        public string UserId { get; set; }
        public Musician author { get; set; }
        
        public UserMusician()
        { }
        public UserMusician(User User,  Musician song)
        {
            user = User;
            Id= Guid.NewGuid();
            UserId = User.Id;
            AuthorId = song.Id;
            author = song;
          
        }
    }
}
