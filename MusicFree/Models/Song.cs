using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MusicFree.Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<SongAuthor> Authors { get; set; }
        public Albumn? Albumn { get; set; }
        
        public string song_filename    { get; set; }
        public Song() { }
        public Song(string name)
        {
            Name = name;
            Authors = new List<SongAuthor>();
             
            }

        }

    }

