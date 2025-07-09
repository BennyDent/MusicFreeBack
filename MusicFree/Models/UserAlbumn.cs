using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models.UserAlbumn
{
    public class UserAlbumn{
        [Key]
        public Guid Id { get; set; }
        public Guid AlbumnId { get; set; }
        public string UserId { get; set; }
        public Albumn Albumn { get; set; }
        public UserAlbumn()
        { }
        public UserAlbumn( string author_id, Albumn albumn)
        {
            Id = Guid.NewGuid();
            AlbumnId = albumn.Id;
            UserId = author_id;
            Albumn = albumn;
        }
    }
}
