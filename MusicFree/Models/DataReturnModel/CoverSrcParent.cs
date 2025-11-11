namespace MusicFree.Models.DataReturnModel
{
    public class CoverSrcParent: IdReturnParent
    {

        public string cover_src {  get; set; }


        public CoverSrcParent()
        {

        }


        public CoverSrcParent(string name, Guid id, string coverSrc) : base(name, id)
        {

            cover_src = coverSrc;


        }
        

    }
}
