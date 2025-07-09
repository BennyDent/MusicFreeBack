using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class TagCollection
    {
        [Key]
        public Guid Id { get; set; }
      
        public Tags tag { get; set; }

        public string tag_id { get; set; }
        public TagCollection() { }
        public TagCollection(Tags Tag)
        {
            this.Id = Guid.NewGuid();

            tag = Tag;
            tag_id = Tag.Name;


           

        }
    }
}
