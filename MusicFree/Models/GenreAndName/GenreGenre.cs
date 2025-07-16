using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class GenreGenre
    {
        public Guid Id { get; set; }
        public ICollection<Genre> genres;

        public GenreGenre() { }

        public GenreGenre(Genre First, Genre Second)
        {
          Id = Guid.NewGuid();
          genres = new List<Genre>();
          genres.Add(First);
        genres.Add(Second);
        }

    }
}
