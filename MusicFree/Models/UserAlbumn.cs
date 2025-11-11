using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class UserAlbumn
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AlbumnId { get; set; }

        public User user { get; set; }
        public string UserId { get; set; }
        public Albumn Albumn { get; set; }
        public UserAlbumn()
        { }
        public UserAlbumn(Albumn albumn, User User)
        {
            Id = Guid.NewGuid();
            AlbumnId = albumn.Id;
            UserId = User.Id;
            Albumn = albumn;
            user = User;
        }
    }
}
