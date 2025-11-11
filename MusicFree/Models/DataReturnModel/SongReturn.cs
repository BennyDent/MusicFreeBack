using MongoDB.Bson.Serialization.IdGenerators;

namespace MusicFree.Models.DataReturnModel
{
    public class SongReturn: CoverSrcParent
    {
     
        
        public AuthorReturn main_author { get; set; }
        public List<AuthorReturn> extra_authors { get; set; }
        public string file_src { get; set; }
        public bool? is_liked { get; set; }
        public Guid? albumn_id { get; set; }


        public SongReturn(string name, Guid id, string cover_src) :base(name,id,cover_src)
        {

        }


        public SongReturn(Song song, bool? isliked) {
            cover_src = song.cover_src;
            Console.WriteLine(song);
            main_author = new AuthorReturn(song.Main_Author);
            extra_authors = new List<AuthorReturn>();
            file_src= song.file_src;
            foreach (var extra in song.extra_authors)
            {
                extra_authors.Add(new AuthorReturn(extra.Author));
            }
            albumn_id = song.Albumn.Id;
            Id = song.Id;
            Name = song.Name;

            if (isliked != null)
            {
                is_liked = isliked;
            }


        }
    }
}
