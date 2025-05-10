using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class GenreAndName
    {

        [Key]
        public string Name { get; set; }
        public  ICollection<GenreAndNameCollection> songs { get; set; }

        public ICollection<GenreAndNameCollection> similar { get; set; }
        public GenreAndName()
        { }
        public GenreAndName(string name)
        {
        
        songs = new List<GenreAndNameCollection>();
        similar = new List<GenreAndNameCollection>();
        Name = name;
        }
    }
}
