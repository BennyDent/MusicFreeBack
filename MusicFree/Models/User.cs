using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
namespace MusicFree.Models
{
    public class User 
    {
        [Key]
        public string Id { get; set; }
        public ICollection<UserSong> song_likes { get; set; }

        public ICollection<UserMusician> author_subcription { get; set; }

        public ICollection<UserAlbumn> liked_albumns { get; set; }


        public ICollection<MusicianView> authors_view { get; set; } 

        public IList<AlbumnViews> albumn_views { get; set; }
        public ICollection<SongViews> song_views { get; set; }

        public IList<Guid> last_search { get; set; }

      

        public ICollection<Playlist> playlists { get; }

        public string username { get; set; }

        public string? email { get; set; } 

        public UserRadio radio { get; set; }



        [ForeignKey("RadioId")]
        public Guid RadioId   { get; set; }


        public string confirm_code { get; set; }
    public User() {

            song_likes = new List<UserSong>();
            song_views = new List<SongViews>();
            last_search = new List<Guid>();
            albumn_views = new List<AlbumnViews>();
            
        }
      public User( string id, string username,  string? email)
        {
            playlists = new List<Playlist>();
            Id = id;
            username = username;
            email = email;
            radio = new UserRadio();
            RadioId = radio.Id;
            
        }
    }
}
