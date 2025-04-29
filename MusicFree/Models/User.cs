using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
namespace MusicFree.Models
{
    public class User : IdentityUser
    {
        public IList<Guid> song_likes { get; set; }

        public IList<Guid> author_subcription { get; set; }

        public IList<Guid> song_views { get; set; }

        public IList<Guid> last_search        {get;set; }

        public IList<Guid> albumn_views { get; set; }

        public IList<Guid> playlists { get; } = new List<Guid>();

        public string confirm_code { get; set; }
    public User() {

            song_likes = new List<Guid>();
            song_views = new List<Guid>();
        }
      public User(string email, string username, string Confirm_code)
        { 
             
            song_likes = new List<Guid>();  
            author_subcription = new List<Guid>();
            song_views = new List<Guid>();
            SecurityStamp = Guid.NewGuid().ToString();
            Email = email;
            UserName = username;
            confirm_code = Confirm_code;
            last_search = new List<Guid>(); 
            albumn_views = new List<Guid>();
        }
    }
}
