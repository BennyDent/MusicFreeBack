namespace MusicFree.Models
{
    public class GenreAndName
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public  ICollection<Song> songs { get; set; }
        public GenreAndName()
        { }
        public GenreAndName(string name)
        {
        Id = Guid.NewGuid();
        songs = new List<Song>();
        Name = name;
        }
    }
}
