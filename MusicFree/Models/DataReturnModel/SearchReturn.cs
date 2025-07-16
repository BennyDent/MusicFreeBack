namespace MusicFree.Models.DataReturnModel
{
    public class SearchReturn
    {
        public DateTime last_searched { get; set; }

        public ReturnParent returnParent {get; set;}
        public SearchReturn(DateTime LastSearched, ReturnParent ReturnParent )
        {
            last_searched = LastSearched;
            returnParent = ReturnParent;
        }

    }

    public class SongSearchReturn:SearchReturn 
    {
        public SongReturn SongReturn { get; set; }


        public SongSearchReturn(DateTime lastsearched, SongReturn songReturn) : base(lastsearched)
        {
            SongReturn = songReturn;
        }

        }
    public class AlbumnSearchReturn : SearchReturn
    {
        public AlbumnReturn AlbumnReturn { get; set; }


        public AlbumnSearchReturn(DateTime lastsearched, AlbumnReturn songReturn) : base(lastsearched)
        {
           AlbumnReturn = songReturn;
        }

    }
    public class AuthorSearchReturn : SearchReturn
    {
        public AuthorReturn AuthorReturn { get; set; }


        public AuthorSearchReturn(DateTime lastsearched, AuthorReturn songReturn) : base(lastsearched)
        {
            AuthorReturn = songReturn;
        }

    }



}
