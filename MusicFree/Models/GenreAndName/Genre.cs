using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class Genre: GenreTag
    {
        [Key]
       
        public ICollection<GenretoAlbumn> albumns { get; set; }
        public ICollection<GenretoSong> song { get; set; }
        public ICollection<GenreUser> users { get; set; }
        public ICollection<GenretoMusician> authors { get; set; }
        public ICollection<GenreGenre> similar { get; set; }
        public Genre(string name): base(name)
        {   similar = new List<GenreGenre>();
            
            albumns = new List<GenretoAlbumn>();
            song = new List<GenretoSong>();
            users = new List<GenreUser>();
            authors = new List<GenretoMusician>();
          
        }

        public Genre()
        {
            similar = new List<GenreGenre>();

            albumns = new List<GenretoAlbumn>();
            song = new List<GenretoSong>();
            users = new List<GenreUser>();
            authors = new List<GenretoMusician>();
        }


    }
}
