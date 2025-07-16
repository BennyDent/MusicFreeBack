using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class GenreCollection: GenreTagCollection
    {
        [Key]
        public Guid Id { get; set; }
       

        public Genre genre { get; set; }
        public string genre_id { get; set; }
        public GenreCollection() { }
        
    }
}
