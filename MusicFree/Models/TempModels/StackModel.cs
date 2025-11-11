
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MusicFree.Models.TempModels
{
    [Table("stackModel")]
    public class StackModel: ITempTable
    {


        [Key]
        public Guid Id { get; set; }

        public string user_id { get; set; }

        public Guid song_id { get; set; }

        public DateTime timestamp { get; set; }

        public StackModel()
        {
            Id = Guid.NewGuid();
            timestamp = DateTime.Now;

        }


        public StackModel(Guid song, string user)
        {

            Id = Guid.NewGuid();
            timestamp = DateTime.Now;
            user_id = user;
            song_id = song;

        }

    }


    public class StackModelDTO: StackModel
    {


        public StackModelDTO(StackModel input)
        {
            timestamp = input.timestamp;
            Id = input.Id;
            user_id = input.user_id;
            song_id = input.song_id;


        }
    }


}
