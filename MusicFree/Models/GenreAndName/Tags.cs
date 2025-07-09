using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class Tags: GenreTag
    {
        [Key]
        public string Name { get; set; }
        public ICollection<TagtoAlbumn> albumns { get; set; }
        public ICollection<TagtoSong> song { get; set; }

        public ICollection<TagTag> similar { get; set; }
        public ICollection<TagUser> users { get; set; }

        public Tags(string name) {
            Name = name;
        albumns = new List<TagtoAlbumn>();
        song = new List<TagtoSong>();
        users = new List<TagUser>();
        similar = new List<TagTag>();
        }

        
    }
    
}
