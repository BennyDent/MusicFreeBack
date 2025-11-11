using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class MusicianView:ViewClass
    {

        [Key]
        public Guid Id { get; set; }
        public Guid MusicianId { get; set; }
        public string UserId { get; set; }
        
        public User user { get; set; }
        
        public Musician author { get; set; }

     
        public MusicianView()
        { }
        public MusicianView( User user, Musician song): base()
        {
            Id = Guid.NewGuid();
            author = song;
            MusicianId = song.Id;
            UserId = user.Id;
            last_listened = DateTime.Now;


        }
    }
}
