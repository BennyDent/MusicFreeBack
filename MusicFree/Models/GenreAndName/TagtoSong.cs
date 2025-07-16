using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class TagtoSong : TagCollection
    {
      public Song song { get; set; }

      public Guid song_id { get; set; }

        public TagtoSong() { }

        public TagtoSong(Tags _tag, Song Song)
        {
           song = Song;
            song_id = Song.Id;
            tag = _tag;
            tag_id = _tag.Name;
            Id = Guid.NewGuid();

        }

    }
}
