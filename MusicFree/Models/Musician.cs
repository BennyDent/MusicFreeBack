using Microsoft.EntityFrameworkCore;
using MusicFree.Models.GenreAndName;
using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class Musician: MainParent
    {

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<SongAuthor> collaboration_songs { get; set; }
        public ICollection<AlbumnAuthor> collaboration_albumns { get; set; }
        public ICollection<Albumn> Albumns { get; set; }
        public ICollection<MusicianUser> liked_by { get; set; }

        public ICollection<GenretoMusician> genres { get; set; }

        public ICollection<MusicianLastSearch> lastSearch { get; set; } 

        public ICollection<MusicianView> listened_by { get; set; }

        
        public string cover_src { get; set; }

        public string img_filename { get; set; }
       public Musician() {
            liked_by = new List<MusicianUser>();
            Songs = new List<Song>();
        }
       public Musician(string name, string src)
        {

            lastSearch = new List<MusicianLastSearch>();
            genres = new List<GenretoMusician>();
            
           
            cover_src = src;
            
            img_filename = Guid.NewGuid().ToString();
            listened_by = new List<MusicianView>();
            Name = name;
            Songs = new List<Song>();
            collaboration_songs = new List<SongAuthor>();
            Albumns = new List<Albumn>();
            collaboration_albumns = new List<AlbumnAuthor>();
            liked_by = new List<MusicianUser>();
            this.img_filename = img_filename;
        }
    }
}
