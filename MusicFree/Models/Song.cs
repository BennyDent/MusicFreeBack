using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MusicFree.Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Musician Main_Author { get; set; }
        public ICollection<SongAuthor>? extra_authors { get; set; }
        public Albumn? Albumn { get; set; }
        
        public  int? albumn_index { get; set; }

        public string song_filename    { get; set; }
        public Song() { }
        public Song(string name, Musician main_author )
        {
            Name = name;
            extra_authors = new List<SongAuthor>();
            main_author = main_author;
            }

        }

    }

