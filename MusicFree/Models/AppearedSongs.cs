using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class AppearedSongs
    {
        [Key]
        public string UserId { get; set; } 
        
        public Array<Guid> Used_Songs_Ids { get; set; }

        public ApearedSongs() { }
    }
}
