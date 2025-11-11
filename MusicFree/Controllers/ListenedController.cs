using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using MusicFree.Models;
using MusicFree.Models.GenreAndName;
using MusicFree.Models.DataReturnModel;
using MusicFree.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Asn1.X509.Qualified;
using MusicFree.Models.ExtraModels;
using System.Threading.Tasks;
using MusicFree.Models.InputModels;
namespace MusicFree.Controllers
{
    public class ListenedController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserContext _userContext;
        private SongReturn _songRethurn;
        private bool is_Changed;
        private MusicService _ms;
        private ContextMusicService _cms;
        public ListenedController(UserManager<IdentityUser> userManager, FreeMusicContext context, UserContext userContext)
        {
           
            _context = context;
            _ms = new MusicService();
            _cms = new ContextMusicService(userManager, context);
        }


      

        [NonAction]
        public async Task AddSearched( Song listened, User user) { 
        
           bool AddorEdit( List<LastSearchParent> lastsearches)
            {
                if (lastsearches.Where(a => a.UserId == user.Id).Any())
                {

                    lastsearches.Where(a => a.UserId == user.Id).First().last_searched = DateTime.Now;
                    return false;
                }
                else { return true; }
            }

var song_listened = (Song)listened;
                if (AddorEdit(song_listened.lastSearch.Cast<LastSearchParent>().ToList()))
                {
                    var new_last_search = new SongLastSearch(user.Id, song_listened);
                    _context.songlastSearches.Add(new_last_search);
                    song_listened.lastSearch.Add(new_last_search);
                }

           
                if (AddorEdit(listened.Albumn.lastSearch.Cast<LastSearchParent>().ToList()))
                {
                    var new_last_search = new AlbumnLastSearch(user.Id, listened.Albumn);
                    _context.albumnlastSearches.Add(new_last_search);

                    listened.Albumn.lastSearch.Add(new_last_search);
                }


           
                if (AddorEdit(listened.Main_Author.lastSearch.Cast<LastSearchParent>().ToList()))
                {
                    var new_last_search = new MusicianLastSearch(user.Id, listened.Main_Author);

                    _context.musicianlastSearches.Add(new_last_search);
                    listened.Main_Author.lastSearch.Add(new_last_search);
                
                await _context.SaveChangesAsync();
            }

         


       
        
        
        }

        [NonAction]
        public async  Task ListenedAlbumn(Albumn albumn, User user)
        {
            var is_listened = (albumn.Songs.Where(a => a.song_views.Where(a => a.UserId == user.Id).Any()).Count() > 1 && albumn.albumn_type == AlbumnType.albumn)
                    || albumn.albumn_type != AlbumnType.albumn;
            if (albumn.albumn_views.Where(a=> a.UserId == user.Id).Any())
            {
               var to_change = albumn.albumn_views.Where(a => a.UserId == user.Id).First();
                 to_change.last_listened = DateTime.Now;
                if (is_listened)
                {
                    
                    to_change.listened++;
                }
               


            }
            else
            {
                if((albumn.Songs.Where(a=> a.song_views.Where(a=> a.UserId==user.Id).Any()).Count()  > 1&& albumn.albumn_type==AlbumnType.albumn)
                    || albumn.albumn_type!=AlbumnType.albumn)
                {

                    if (is_listened)
                    {
    var albumn_view = new AlbumnViews(user,albumn );
                    albumn.albumn_views.Add(albumn_view);
                    _context.albumn_views.Add(albumn_view);
                    }
                

                }
            }
            await _context.SaveChangesAsync();
        }


        [NonAction]
        public async Task MusicianListened(Musician musician,User user, List<SongAuthor>? extra_authors )
        {
            bool SongMusician(Musician a)
            {


                return _context.songsViews.Where(b =>user.Id==b.UserId &&( b.song.Main_Author.Id == a.Id|| b.song.extra_authors.Where(c=>c.AuthorId==a.Id).Any())).Count() > 1;
           }


            async Task AuthorviewAdd(Musician a)
            {

                var result = SongMusician(a);

                if (a.musician_views.Where(b=>b.UserId==user.Id).Any()) {

                   var  to_change = musician.musician_views.Where(b=> b.MusicianId == a.Id).First();
                    to_change.last_listened = DateTime.Now;
                    if (result)
                    {
                        to_change.listened++;
                    }

                } else
                {

                if (result) {
                        var new_view = new MusicianView(user, musician);
                        _context.userMusicians.Add(new_view);
                        a.musician_views.Add(new_view);

                    }

               
                }

            }

            await AuthorviewAdd(musician);
            if(extra_authors != null)
            {
  foreach(var extra_author in extra_authors)
            {
               await AuthorviewAdd(extra_author.Author);
            }
            }
          

 await _context.SaveChangesAsync();

        }

        [Authorize]
        [Route("music/listened/add")]
        [HttpPost]
        public async Task<ActionResult> AddListen(ListenedInput input)
        {

var user = await _cms.ReturnUserModel(HttpContext.User);

            var song = await _context.songs.FindAsync(input.song_id);

            await ListenedAlbumn(song.Albumn, user);

            await MusicianListened(song.Main_Author,user, song.extra_authors.ToList() );

            if (input.is_searched)
            {
                await AddSearched(song, user);
            }



            return Ok();
        }
        



    }
}
