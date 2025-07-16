using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class TagTag
    {
        [Key]
        public Guid Id { get; set; }
      public ICollection<Tags> tags { get; set; }

        public TagTag() { }

        public TagTag(Tags First, Tags Second)
        {   
            tags = new List<Tags>();
            tags.Add(First);
            tags.Add(Second);
        }

    }
}
