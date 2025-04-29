using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class SearchModel
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public bool is_albumn { get; set; }

        public DateTime timestamp { get; set; }
        public Albumn? Albumn { get; set; }
        public Musician? Author { get; set; }

        public SearchModel() { }
        public SearchModel(User user, bool albumn_is, Albumn albumn)
        {
            is_albumn = albumn_is;
            Id = Guid.NewGuid();    
           Albumn = albumn;
            UserId = user.Id;    
        }
        public SearchModel(User user, bool albumn_is, Musician musician)
        {
            is_albumn = albumn_is;
            Id = Guid.NewGuid();
            Author = musician;
            UserId = user.Id;
        }
    }
}
