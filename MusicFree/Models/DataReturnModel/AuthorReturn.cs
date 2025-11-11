namespace MusicFree.Models.DataReturnModel
{
    public class AuthorReturn: CoverSrcParent
    {

      
      
        public AuthorReturn(Musician musician)
        {
            Id = musician.Id;
            Name = musician.Name;
            cover_src = musician.cover_src;
        }


        public AuthorReturn( string Id, string Name, string cover_src)
        {


        }
    }
}
