using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class TagCollection: GenreTagCollection
    {
        [Key]
        public Guid Id { get; set; }
      
        public Tags tag { get; set; }

        public string tag_id { get; set; }
        public TagCollection() { }
        
    }
}
