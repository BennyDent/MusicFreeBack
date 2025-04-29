namespace MusicFree.Models
{
    public class GenreAndNameCollection
    {
        public ICollection<GenreAndName> genre_and_name { get; set; }
        public ICollection<Song> song { get; set; }
        public Guid genre_and_name_id { get; set; }
        public Guid song_id { get; set; }
        public GenreAndNameCollection(){
        
}
        public GenreAndNameCollection( Guid Genre_and_name_id, Guid SongId)
        {
            genre_and_name = new List<GenreAndName>();
            song = new List<Song>();
            song_id = SongId;
            genre_and_name_id = Genre_and_name_id;
        }
    }
}
