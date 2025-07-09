namespace MusicFree.Models.InputModels
{
    public class GenreTagInput
    {
        public string name {  get; set; }
        public Boolean is_tag { get; set; } 
      public List<string> similar {  get; set; }
    }
}
