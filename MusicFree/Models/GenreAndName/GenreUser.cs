using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    // жанры и тэги прослушанные
    public class GenreAndName_User
    {
        [Key]
        public Guid Id { get; set; }
        public string user_id { get; set; }

       

        public int listened { get; set; }

        public DateTime last_listened { get; set; }

        public GenreAndName_User()
        {

           
        }

        public GenreAndName_User(string User_Id)
        {
            Id = Guid.NewGuid();
            user_id = User_Id;
            listened = 0;
            last_listened = DateTime.Now;
        }

       



    }
    public class GenreUser: GenreAndName_User
        {
             public string genre_id { get; set; }
            public Genre genre { get; set; }
            public GenreUser() { }
            public GenreUser(string User_Id, Genre Genre) {
        genre = Genre;
        genre_id = Genre.Name;
        }
        }

        public class TagUser: GenreAndName_User
    {

        public string tag_id { get; set; }
        public Tags tag { get; set; }
        public TagUser() { }
        public TagUser (string User_Id, Tags Genre)
        {
            tag = Genre;
            tag_id = Genre.Name;
            
        }

    }
}
