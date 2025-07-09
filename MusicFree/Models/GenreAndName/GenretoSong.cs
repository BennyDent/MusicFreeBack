namespace MusicFree.Models.GenreAndName
{
    public class GenretoSong : GenreCollection
    {
        public Song song { get; set; }

        public Guid song_id { get; set; }

        public GenretoSong() { }

        public GenretoSong(Genre genre, Song Song)
        {
            song = Song;
            song_id = Song.Id;

        }
    }
}
