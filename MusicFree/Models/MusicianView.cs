using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class MusicianView
    {

        [Key]
        public Guid Id { get; set; }
        public Guid MusicianId { get; set; }
        public string UserId { get; set; }
        public Musician author { get; set; }
        public MusicianView()
        { }
        public MusicianView( string user_id, Musician song)
        {
            Id = Guid.NewGuid();
            author = song;
            MusicianId = song.Id;
            UserId = user_id;
            

        }
    }
}
