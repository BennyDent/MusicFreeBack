using Microsoft.EntityFrameworkCore;
using MusicFree.Models.GenreAndName;
using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class Albumn
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Required]
        public ICollection<TagtoAlbumn> tags { get; set; }

        [Required]
        public ICollection<GenretoAlbumn> genres { get; set; }

        public Musician Main_Author { get; set; }
        public ICollection<AlbumnAuthor>  Extra_Authors { get; set; }
        public ICollection<Song> Songs { get; set; }
        public Boolean is_visible { get; set; }
        public string cover_filename { get; set; }
        
    

        public ICollection<AlbumnViews> albumn_views { get; set; }

        public Albumn() {
        Songs = new List<Song>();
         albumn_views = new List<AlbumnViews>();
            Extra_Authors = new List<AlbumnAuthor>();
        }
        public Albumn( string name, Musician main_author)
        {
            
            albumn_views = new List<AlbumnViews>();
            Id = Guid.NewGuid();
            Name = name;
            Main_Author = main_author;
            Extra_Authors = new List<AlbumnAuthor>();
            Songs = new List<Song>();
            cover_filename = Guid.NewGuid().ToString();
        }
    }
}
