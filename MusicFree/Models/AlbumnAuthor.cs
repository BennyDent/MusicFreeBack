namespace MusicFree.Models
{
    public class AlbumnAuthor
    {
        public Guid AlbumnId { get; set; }
        public Guid AuthorId { get; set; }
        public Albumn Albumn { get; set; }
        public Musician Author { get; set; }
        public AlbumnAuthor()
        { }
        public AlbumnAuthor(Guid song_id, Guid author_id, Albumn song, Musician author)
        {
            AlbumnId = song_id;
            AuthorId = author_id;
            Albumn = song;
            Author = author;
        }
    }
}
