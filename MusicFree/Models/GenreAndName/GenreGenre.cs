namespace MusicFree.Models.GenreAndName
{
    public class GenreGenre
    {
        public Genre first { get; set; }
        public Genre second { get; set; }
        public string first_id { get; set; }
        public string second_id { get; set; }

        public GenreGenre() { }

        public GenreGenre(Genre First, Genre Second)
        {

            first = First;
            second = Second;
            first_id = First.Name;
            second_id = First.Name;
        }

    }
}
