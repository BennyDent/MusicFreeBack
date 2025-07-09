using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class GenreCollection
    {
        [Key]
        public Guid Id { get; set; }
        public Song song { get; set; }
        public Guid song_id { get; set; }

        public Genre genre { get; set; }
        public string genre_id { get; set; }
        public GenreCollection() { }
        public GenreCollection(Genre Genre)
        {
            this.Id = Guid.NewGuid();
            song = song1;
            song_id = song1.Id;
            genre = Genre;
            genre_id = Genre.Name;
        }
    }
}
