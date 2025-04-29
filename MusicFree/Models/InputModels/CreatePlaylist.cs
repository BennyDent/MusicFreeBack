namespace MusicFree.Models.AutenthicationModels
{
    public class CreatePlaylist
    {
        public string name {  get; set; }  
        public ICollection<Guid> songs { get; set; }
    }
}
