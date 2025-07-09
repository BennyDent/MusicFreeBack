namespace MusicFree.Models.GenreAndName
{
    public class TagtoAlbumn: TagCollection
    {
        public Albumn albumn { get; set; }
        public Guid albumn_id { get; set; }

        public TagtoAlbumn() { }
     
        public TagtoAlbumn(Tags Tag, Albumn Albumn) {
            
            albumn = Albumn;
            albumn_id = Albumn.Id;
        
        }
       
    }
}
