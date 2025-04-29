namespace MusicFree.Models
{
    public class SongForm
    {
        public string name { get; set; }

        public Guid main_author { get; set; }
        public List<Guid> extra_authors {get;set; }
       public int index { get; set; }

   
    }
}
