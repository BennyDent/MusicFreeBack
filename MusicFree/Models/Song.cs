using MongoDB.Bson;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using MusicFree.Models.GenreAndName;
using System.ComponentModel.DataAnnotations.Schema;
namespace MusicFree.Models
{
    public class Song: AutoIncrementedParent
    {
        [Key]
       public Guid Id  { get; set; }
        public string Name { get; set; }

        [Required]
        public ICollection<TagtoSong> tags  { get; set; }
        [Required]
        public ICollection<GenretoSong> genres { get; set; }

        public string file_src { get; set; }
        public Musician Main_Author { get; set; }
        public ICollection<SongAuthor>? extra_authors { get; }



        

        public ICollection<PlaylistSong> playllists { get; } = new List<PlaylistSong>();

        [Required]
        public Albumn Albumn { get; set; }

        [Required]
        public ICollection<UserSong> liked_by { get; }
        public  int albumn_index { get; set; }
        public int listened {  get; set; }  
        public string cover_src { get; set; }
        public ICollection<SongLastSearch> lastSearch { get; set; }
        public ICollection<SongViews> song_views { get; set; } 



        public Song() {
            liked_by = new List<UserSong>();
            song_views = new List<SongViews>();
            extra_authors = new List<SongAuthor>();
            
        }
        public Song(string name, Musician main_author, Albumn albumn)
        {

            genres = new List<GenretoSong>();
            tags = new List<TagtoSong>();
            cover_src = albumn.cover_src;
            file_src = Guid.NewGuid().ToString();
            Name = name;
            lastSearch = new List<SongLastSearch>();
            extra_authors = new List<SongAuthor>();
            Main_Author = main_author;
            liked_by = new List<UserSong>();
            song_views = new List<SongViews>();
            Albumn = albumn;
            }

        }

    }

