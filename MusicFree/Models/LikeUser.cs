namespace MusicFree.Models
{
    public class LikeUser
    {
        public Guid LikeId { get; set; }
        public Guid UserId { get; set; }
        public Like like { get; set; }
        public User User { get; set; }
    }
}
