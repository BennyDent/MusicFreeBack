namespace MusicFree.Models
{
    public class SongReturn
    {
        public string filename { get; set; }
        public string Name { get; set; }
        public AuthorReturn main_author { get; set; }
        public List<AuthorReturn> extra_authors { get; set; }
        public Guid Id { get; set; }
        public bool is_liked { get; set; }
        public Guid? albumn_id { get; set; }

        public SongReturn(Guid id, AuthorReturn author, List<AuthorReturn> authors, string Filename, string name, bool is_like) {

            filename = Filename;
            Name = name;
            main_author = author;
            extra_authors = authors;
            Id = id;
          is_liked= is_like;
        }
    }
}
