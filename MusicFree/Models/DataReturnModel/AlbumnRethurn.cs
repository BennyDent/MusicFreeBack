namespace MusicFree.Models.DataReturnModel
{
    public class AlbumnReturn:CoverSrcParent
    {
       
        public AuthorReturn main_author { get; set; }
        public List<AuthorReturn> extra_author { get; set; }
      
     
        public AlbumnReturn(Albumn albumn)
        {

            Id = albumn.Id; 
            Name = albumn.Name;
            main_author = new AuthorReturn(albumn.Main_Author);
            
            extra_author = new List<AuthorReturn>();

            foreach (var extra in albumn.Extra_Authors)
            {
                extra_author.Add(new AuthorReturn(extra.Author));
            }
            cover_src = albumn.cover_src;
        }
    }
}
