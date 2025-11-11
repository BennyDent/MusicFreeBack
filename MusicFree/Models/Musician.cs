using Microsoft.EntityFrameworkCore;
using MusicFree.Models.GenreAndName;
using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class Musician: AutoIncrementedParent
    {

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<SongAuthor> collaboration_songs { get; set; }
        public ICollection<AlbumnAuthor> collaboration_albumns { get; set; }
        public ICollection<Albumn> Albumns { get; set; }
        public ICollection<UserMusician> liked_by { get; set; }

        public ICollection<GenretoMusician> genres { get; set; }

        public ICollection<MusicianLastSearch> lastSearch { get; set; } 

        public ICollection<MusicianView> musician_views { get; set; }

        
        public string cover_src { get; set; }

      
       public Musician() {
            liked_by = new List<UserMusician>();
            Songs = new List<Song>();
            musician_views = new List<MusicianView>();
            liked_by = new List<UserMusician>();
        }
       public Musician(string name)
        {

            lastSearch = new List<MusicianLastSearch>();
            genres = new List<GenretoMusician>();
            
           
            cover_src = Guid.NewGuid().ToString();
            
           
           musician_views = new List<MusicianView>();
            Name = name;
            Songs = new List<Song>();
            collaboration_songs = new List<SongAuthor>();
            Albumns = new List<Albumn>();
            collaboration_albumns = new List<AlbumnAuthor>();
            liked_by = new List<UserMusician>();
        }
    }
}
