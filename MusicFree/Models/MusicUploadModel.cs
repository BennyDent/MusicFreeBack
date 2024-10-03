namespace MusicFree.Models
{
    public class MusicUploadModel
    {
        public string name { get; set; }
        public Guid author { get; set; }
        public string albumn { get; set; }
        public IFormFile song { get; set; }
    }
}
