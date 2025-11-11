
using Org.BouncyCastle.Bcpg;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MusicFree.Models
{
    public class UserRadio
    {
        [Key]
    public Guid Id { get; set; }

  
     public User user { get; set; }

    public string user_id { get; set; }
  
 
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
        
           
            same_author_possibility = 40;
        }

    }
    

}
