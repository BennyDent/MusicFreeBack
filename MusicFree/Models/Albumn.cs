namespace MusicFree.Models
{
    public class Albumn
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Musician Main_Author { get; set; }
        public ICollection<AlbumnAuthor>  Extra_Authors { get; set; }
        public ICollection<Song> Songs { get; set; }
        public Boolean is_visible { get; set; }
        public Albumn() { }
        public Albumn( string name, Musician main_author)
        {
            Id = Guid.NewGuid();
            Name = name;
            Main_Author = main_author;
           Extra_Authors = new List<AlbumnAuthor>();
            Songs = new List<Song>(); 
        }
    }
}
