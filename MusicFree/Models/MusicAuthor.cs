using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class SongAuthor
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SongId { get; set; }
        public Guid AuthorId { get; set; }
        public Song Song { get; set; }
        public Musician Author { get; set; }
        public SongAuthor()
        { }
        public SongAuthor(Guid song_id, Guid author_id, Song song, Musician author) {
            Id = Guid.NewGuid();
            SongId = song_id;  
        AuthorId = author_id;
        Song = song;
        Author = author;
        }
    }
}
