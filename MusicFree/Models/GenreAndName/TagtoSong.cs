using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class TagtoSong : TagCollection
    {
      public Song song { get; set; }

      public Guid song_id { get; set; }

        public TagtoSong() { }

        public TagtoSong(Tags tag, Song Song)
        {
           song = Song;
            song_id = Song.Id;

        }

    }
}
