using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models.GenreAndName
{
    public class GenreAndNameCollection
    {
        [Key]
        public Guid Id { get; set; }
        public Tags tag { get; set; }
        public Guid tag_id { get; set; }

        public Genre genre { get; set; }

        public Guid genre_id { get; set; }
        public GenreAndNameCollection() {
        
        }
    }
}
