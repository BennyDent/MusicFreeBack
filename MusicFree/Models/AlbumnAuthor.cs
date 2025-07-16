using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class AlbumnAuthor
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AlbumnId { get; set; }
        public Guid AuthorId { get; set; }
        public Albumn Albumn { get; set; }
        public Musician Author { get; set; }
        public AlbumnAuthor()
        { }
        public AlbumnAuthor( Albumn song, Musician author)
        {
            AlbumnId = song.Id;
            AuthorId = author.Id;
            Albumn = song;
            Author = author;
        }
    }
}
