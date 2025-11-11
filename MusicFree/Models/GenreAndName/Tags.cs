using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class Tags: GenreTag
    {
     
        
        public ICollection<TagtoAlbumn> albumns { get; set; }
        public ICollection<TagtoSong> song { get; set; }

        public ICollection<TagTag> similar { get; set; }
        public ICollection<TagUser> users { get; set; }


        public Tags()
        {
            albumns = new List<TagtoAlbumn>();
            song = new List<TagtoSong>();
            users = new List<TagUser>();
            similar = new List<TagTag>();
        }


        public Tags(string name): base(name) {
           
        albumns = new List<TagtoAlbumn>();
        song = new List<TagtoSong>();
        users = new List<TagUser>();
        similar = new List<TagTag>();
        }

        
    }
    
}
