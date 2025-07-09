namespace MusicFree.Models.GenreAndName
{
    public class GenretoAlbumn: GenreCollection
    {
        public Albumn albumn { get; set; }

        public Guid albumn_id { get; set; }

        public GenretoAlbumn() { }

        public GenretoAlbumn(Tag tag, Albumn Albumn)
        {
            albumn = Albumn;
            albumn_id = Albumn.Id;

        }
    }
