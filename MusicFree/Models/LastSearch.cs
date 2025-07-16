using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class LastSearchParent
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime last_searched {get; set; }

    }

    public class SongLastSearch: LastSearchParent
    {
    
    public Song Song { get; set; }
    public Guid SongId { get; set; }

        public SongLastSearch()
        {

        }

        public SongLastSearch(string user_id, Song song)
        {
            Id= Guid.NewGuid();
            SongId= song.Id;
            Song = song;
            UserId= user_id;
            last_searched = DateTime.Now;
        }
    }

    public class AlbumnLastSearch : LastSearchParent
    {

        public Albumn Albumn { get; set; }
        public Guid AlbumnId { get; set; }

        public AlbumnLastSearch()
        {

        }

        public AlbumnLastSearch(string user_id, Albumn song)
        {
            Id = Guid.NewGuid();
            AlbumnId = song.Id;
            Albumn = song;
            UserId = user_id;
            last_searched = DateTime.Now;
        }
    }

    public class MusicianLastSearch : LastSearchParent
    {

        public Musician Author { get; set; }
        public Guid AuthorId { get; set; }

        public MusicianLastSearch()
        {

        }

        public MusicianLastSearch(string user_id, Musician song)
        {
            Id = Guid.NewGuid();
            AuthorId = song.Id;
            Author = song;
            UserId = user_id;
            last_searched = DateTime.Now;
        }
    }


}
