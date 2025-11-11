using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MusicFree.Models
{
    public class AutoIncrementedParent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int auto_increment_index { get; set; }



        public AutoIncrementedParent()
        {
            Console.WriteLine(auto_increment_index);

            Console.WriteLine("detox");
        }
    }
}
