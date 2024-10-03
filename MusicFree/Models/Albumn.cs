namespace MusicFree.Models
{
    public class Albumn
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Musician Author { get; set; }
        public ICollection<Song> Songs { get; set; }
        public Boolean is_visible { get; set; }
        public Albumn() { }
        public Albumn( string name, Musician author)
        {
            Id = Guid.NewGuid();
            Name = name;
            Author = author;
            Songs = new List<Song>(); 
        }
    }
}
