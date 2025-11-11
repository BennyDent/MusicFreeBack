using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{

    public class AlbumnViews: ViewClass
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid AlbumnId { get; set; }

        public User user { get; set; }
      
        public Albumn albumn { get; set; }

       
       
        public AlbumnViews() { }
        public AlbumnViews(User User,  Albumn song): base()
        {
            user = User;
            UserId = user.Id;
            AlbumnId = song.Id;
            listened = 1;
            albumn = song;
        }
    }
}
