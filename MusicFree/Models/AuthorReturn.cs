namespace MusicFree.Models
{
    public class AuthorReturn
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public AuthorReturn(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
