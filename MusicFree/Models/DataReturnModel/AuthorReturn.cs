namespace MusicFree.Models.DataReturnModel
{
    public class AuthorReturn:ReturnParent
    {

      
      
        public AuthorReturn(Musician musician)
        {
            Id = musician.Id;
            Name = musician.Name;
            cover_src = musician.cover_src;
        }
    }
}
