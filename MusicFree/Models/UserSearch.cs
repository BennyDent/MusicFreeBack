using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models
{
    public class UserSearch
    {
        [Key]
        public Guid Id { get; set; }    
        public string UserId { get; set; }
        public Guid searchId { get; set; }
        public DateTime timestamp { get; set; }

        public UserSearch() { }
        public UserSearch(string userid, Guid search, DateTime timestam){
            Id = Guid.NewGuid();
            UserId = userid;
            searchId = search;
            timestamp = timestam;
        }
    }
}
