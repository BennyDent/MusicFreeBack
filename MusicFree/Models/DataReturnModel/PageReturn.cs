namespace MusicFree.Models.DataReturnModel
{
    public class PageReturn
    {

        public bool hasMore {  get; set; }
        public List<ReturnParent> page { get; set; }



        public PageReturn(bool isMore)
        {
            hasMore = isMore;
            page = new List<ReturnParent>();
        }

    }
}
