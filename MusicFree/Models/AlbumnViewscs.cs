using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{

    public class AlbumnViews
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid AlbumnId { get; set; }


      
        public Albumn albumn { get; set; }



        public DateTime last_listened { get; set; }
        public AlbumnViews() { }
        public AlbumnViews(string userId, Guid songId,  Albumn song, DateTime lastlistened)
        {
            last_listened = lastlistened;
            Id = Guid.NewGuid();
            UserId = userId;
            AlbumnId = songId;
          
            this.albumn = song;
        }
    }
}
