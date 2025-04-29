namespace MusicFree.Models.DataReturnModel
{
    public class PaginationreturnData
    {
        public Boolean hasMore { get; set; }
        public dynamic  page {  get; set; }

        public PaginationreturnData(Boolean HasMore,dynamic Page) {
        
            hasMore = HasMore;
            page = Page;    
        }
    }
}
