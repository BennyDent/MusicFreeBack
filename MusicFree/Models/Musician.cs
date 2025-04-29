using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class Musician
    {
       
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<SongAuthor> collaboration_songs { get; set; }
        public ICollection<AlbumnAuthor> collaboration_albumns { get; set; }
        public ICollection<Albumn> Albumns { get; set; }
        public ICollection<MusicianUser> liked_by { get; set; }
        public string img_filename { get; set; }
       public Musician() {
            liked_by = new List<MusicianUser>();
            Songs = new List<Song>();
        }
       public Musician(string name)
        {
            img_filename = Guid.NewGuid().ToString();
            Id = Guid.NewGuid();
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
