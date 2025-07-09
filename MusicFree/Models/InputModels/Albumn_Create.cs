using MusicFree.Models.ExtraModels;
namespace MusicFree.Models.InputModels
{

   
    public class Albumn_Create
    {
        public string name { get; set; }
        public Guid main_author { get; set; }
        public List<Guid> extra_authors { get; set; }
        public List<SongForm> songs { get; set; }
        public int albumn_type { get; set; }

    }
}
