using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
namespace MusicFree.Models
{
    public class UserRadio
    {
        [Key]
    public Guid Id { get; set; }
    public User user { get; set; }
    public List<Guid> radio_history { get; set; }
    public Queue<Guid> radio_stack { get; set; }
    public int radio_index { get; set; }
    public int same_author_possibility { get; set; }
public UserRadio()
    {

    }
  public UserRadio(User new_user)
        {
            Id = Guid.NewGuid();
            radio_index = 0;
            user = new_user;
            radio_history = new List<Guid>();
            radio_stack = new Queue<Guid>();
            same_author_possibility = 40;
        }

    }
    

}
