namespace MusicFree.Models
{
    public class MusicStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string MusicCollectionName { get; set; } = null!;
    }
}
