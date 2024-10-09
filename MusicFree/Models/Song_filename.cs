namespace MusicFree.Models
{
    public class Song_filename
    {
        public string filename {  get; set; }
        public int index { get; set; }
        public Song_filename( string for_filename, int for_index) {
        filename = for_filename;
        index = for_index;  
        }
    }
}
