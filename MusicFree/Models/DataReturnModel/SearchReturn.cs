namespace MusicFree.Models.DataReturnModel
{
    public class SearchReturn
    {
        public DateTime last_searched { get; set; }
        public ReturnParent returnParent { get; set; }
       
        public SearchReturn(DateTime LastSearched, ReturnParent parent)
        {
            last_searched = LastSearched;
            returnParent = parent;
        }

    }

    public class SongSearchReturn:SearchReturn 
    {


        public SongSearchReturn(DateTime lastsearched, ReturnParent parent) : base(lastsearched, parent)
        {
           
        }

        }
    public class AlbumnSearchReturn : SearchReturn
    {
        public AlbumnReturn AlbumnReturn { get; set; }


        public AlbumnSearchReturn(DateTime lastsearched, ReturnParent parent) : base(lastsearched, parent)
        {
          
        }

    }
    public class AuthorSearchReturn : SearchReturn
    {
        public AuthorReturn AuthorReturn { get; set; }


        public AuthorSearchReturn(DateTime lastsearched, ReturnParent parent) : base(lastsearched, parent)
        {
           
        }

    }



}
