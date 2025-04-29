namespace MusicFree.Models
{
    public class AlbumnRethurn
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AuthorReturn main_author { get; set; }
        public List<AuthorReturn> extra_author { get; set; }
      
        public string cover_src { get; set; }
        public AlbumnRethurn(Guid id, string name, AuthorReturn mainauthor, List<AuthorReturn> extraauthors, string src)
        {
            Id = id; 
            Name = name;
            main_author = mainauthor;
            extra_author = extraauthors;
           
            cover_src = src;
        }
    }
}
