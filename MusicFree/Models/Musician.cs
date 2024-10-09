namespace MusicFree.Models
{
    public class Musician
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<SongAuthor> collaboration_songs { get; set; }
        public ICollection<AlbumnAuthor> collaboration_albumns { get; set; }
        public ICollection<Albumn> Albumns { get; set; }
       public Musician() { }
       public Musician(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Songs = new List<Song>();
            collaboration_songs = new List<SongAuthor>();
            Albumns = new List<Albumn>();
            collaboration_albumns = new List<AlbumnAuthor>();

        }
    }
}
