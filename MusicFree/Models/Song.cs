using MongoDB.Bson;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MusicFree.Models
{
    public class Song
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Musician Main_Author { get; set; }
        public ICollection<SongAuthor>? extra_authors { get; }

        public ICollection<PlaylistSong> playllists { get; } = new List<PlaylistSong>();
        public Albumn? Albumn { get; set; }

        [Required]
        public ICollection<UserSong> liked_by { get; }
        public  int? albumn_index { get; set; }

        public string cover_filename { get; set; }
        public string song_filename    { get; set; }
        public ICollection<SongViews> song_views { get; set; } 

        public Song() {
            liked_by = new List<UserSong>();
            song_views = new List<SongViews>();
            extra_authors = new List<SongAuthor>();
            
        }
        public Song(string name, Musician main_author )
        {  cover_filename = Guid.NewGuid().ToString();
            Name = name;
            extra_authors = new List<SongAuthor>();
            Main_Author = main_author;
            liked_by = new List<UserSong>();
            song_views = new List<SongViews>();
            }

        }

    }

