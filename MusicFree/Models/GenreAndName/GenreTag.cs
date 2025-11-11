using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models.GenreAndName
{
    public class GenreTag: AutoIncrementedParent
    {

        [Key]
        public string Name { get; set; }



        public GenreTag()
        {

        }


        public GenreTag(string name)
        {
            Name = name;
        }


    }
}
