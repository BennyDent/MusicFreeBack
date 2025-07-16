namespace MusicFree.Models.GenreAndName
{
    public class GenretoMusician: GenreCollection
    {
        public Musician author { get; set; }
        public Guid author_id { get; set; }

        public GenretoMusician() { }
        public GenretoMusician(Musician Author, Genre Genre) { 
        
        Id = Guid.NewGuid();
        genre = Genre;
            genre_id = Genre.Name;
            author = Author;
            author_id = Author.Id;
        }
    }
}
