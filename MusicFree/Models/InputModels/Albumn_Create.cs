using MusicFree.Models.ExtraModels;
namespace MusicFree.Models.InputModels
{

   
    public class Albumn_Create
    {
        public string name { get; set; }
        public Guid main_author { get; set; }
        public List<Guid> extra_authors { get; set; }
        public List<SongForm> songs { get; set; }
        public AlbumnType albumn_type { get; set; }
        public DateTime date {  get; set; }
        public List<string> genres { get; set; }

        public List<string> tags { get; set; }

    }
}
