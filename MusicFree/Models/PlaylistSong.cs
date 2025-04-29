namespace MusicFree.Models
{
    public class PlaylistSong
    {
        public Guid Id { get; set; }
        public Guid SongId { get; set; }
        public Guid PlaylistId { get; set; }
        public Song Song { get; set; }
        public Playlist Playlist  {get; set; }


        public PlaylistSong()
        {

        }
        public PlaylistSong(Guid songid, Guid playlistid, Song song, Playlist playlist)
        {Id = Guid.NewGuid();
         SongId = songid;
        PlaylistId = playlistid;
        Song = song;
        Playlist = playlist;

        }
    }
}
