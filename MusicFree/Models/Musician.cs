namespace MusicFree.Models
{
    public class Musician
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<SongAuthor> Songs { get; set; }
        public ICollection<Albumn> Albumns { get; set; }
       public Musician() { }
       public Musician(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Songs = new List<SongAuthor>();
            Albumns = new List<Albumn>();

        }
    }
}
