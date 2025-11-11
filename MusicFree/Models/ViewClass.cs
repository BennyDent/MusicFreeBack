namespace MusicFree.Models
{
    public class ViewClass
    {

        public DateTime last_listened;
        public int listened;

        public ViewClass()
        {
            listened = 0;
            last_listened = DateTime.Now;

        }
    }
}
