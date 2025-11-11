using Cottle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver.Linq;
using MusicFree.Migrations;
using MusicFree.Models;
using MusicFree.Models.DataReturnModel;
using MusicFree.Models.GenreAndName;
using MusicFree.Services;
using MusicFree.utilities;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Web;
using ZstdSharp.Unsafe;
namespace MusicFree.Controllers
{

    [ApiController]
    public class MusicFindController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MusicService _ms;
        private readonly ContextMusicService _cms;
        private readonly FreeMusicContext _context;
        private readonly UserContext _userContext;
        public MusicFindController(FreeMusicContext context, UserManager<IdentityUser> userManager)
        {
            _ms = new MusicService();
            _cms = new ContextMusicService(userManager, _context);
            _context = context;
        }
        [Authorize]
        [AllowAnonymous]
        [Route("music/search/author/{name}/{page_size}/{second_coursour}/{third_coursour}")]
        [HttpGet]
        public async Task<ActionResult> AuthorFind(string name, int page_size,  int auto_increment  ,int second_coursor )
        {

            var user = await _cms.ReturnUserModel(HttpContext.User);
            var ms = new MusicService();
            var nextPage = new List<Musician>();
           

            var result = _context.musicians.Where(a=> a.Name.Contains(name)  && a.auto_increment_index > auto_increment && a.musician_views.Count() >= second_coursor)
                .OrderBy(a=>a.musician_views).ThenBy(a=>a.liked_by).Take(page_size).ToList();



            return Ok(SearchPageReturn(result, result.Count<page_size,  result.Last().musician_views.Count(), _context, user.Id));

        }


        [Authorize]
        [AllowAnonymous]
        [Route("music/search/song/{name}/{page_size}/{auto_increment}/{second_coursor}/")]
        [HttpGet]
        public async Task<ActionResult> SongFind(string name, int page_size,  int auto_increment, int second_coursor)
        {
            var ms = new MusicService();
            var nextPage = new List<Albumn>();

            var user = await _cms.ReturnUserModel(HttpContext.User);
            _context.albumns.First().albumn_views = new List<AlbumnViews>();
            await _context.SaveChangesAsync();         
            var result = _context.songs.Where(a => a.Name.Contains(name) && a.auto_increment_index > auto_increment && a.song_views.Count() >= second_coursor)
                .OrderBy(a=>a.song_views.Count()).ThenBy(a=>a.liked_by.Count).Take(page_size).ToList();
            

            return Ok(SearchPageReturn(result, result.Count<page_size, result.Last().song_views.Count(), _context, user.Id));
        }
        [Authorize]
        [AllowAnonymous]
        [Route("music/search/albumn/{name}/{page_size}/{auto_increment}/{second_coursor}")]
        [HttpGet]
        public async Task<ActionResult> AlbumnFind(string name, int page_size,  int auto_increment, int second_coursor) {
            var user = await _cms.ReturnUserModel(HttpContext.User);
            var name_array = name.ToCharArray();
           
            var result = _context.albumns.Where(a => a.Name.Contains(name) && a.auto_increment_index > auto_increment && a.albumn_views.Count() >= second_coursor).OrderBy(a=>a.albumn_views.Count()).ThenBy(a=>a.liked_by.Count())
            .Take(page_size).ToList();
            //
            Console.WriteLine("I am");
            Console.WriteLine(result.First().Main_Author == null);
            
          return Ok(SearchPageReturn(result,result.Count<page_size,result.Last().albumn_views.Count() ,_context,(user==null ? null:user.Id)));

      }

        [NonAction]
        static object SearchPageReturn<T>(List<T> result, bool hasMore, int third_coursor, FreeMusicContext context, string? UserId)  where T :  AutoIncrementedParent
        {
            var is_musician = false;
            var is_albumn = false;


            Console.WriteLine(result.First().GetType().Name);
            Console.WriteLine((result.First() as Albumn).Main_Author==null);
            var for_return = new List<ReturnParent>();
            foreach(var part in result)
            {

                switch (part.GetType().Name) {
                    case "Musician":
                    is_musician = true;
                    for_return.Add(new AuthorReturn(part as Musician));
                        break;

                    case "Albumn":
                        is_albumn = true;
              for_return.Add(new AlbumnReturn(part as Albumn));
                        break;
                    case "Song":
 var song = part as Song;
                    var is_liked = false;
                    if(UserId != null)
                    {
                      
                        is_liked = context.songsViews.Where(a=> a.UserId==UserId && song.Id==a.Id).Any();
                    }

                    for_return.Add(new SongReturn(song, is_liked));
                    break;
                }

            }
            int[] coursour = { result.Last().auto_increment_index, third_coursor };


            if (is_musician)
            {
                return new { hasMore = hasMore, coursours = coursour, page = for_return.Cast<AuthorReturn>().ToList() };
            }
            else if (is_albumn)
            {
                return new { hasMore = hasMore, coursours = coursour, page = for_return.Cast<AlbumnReturn>().ToList() };
            }
            else
            {
 return new { hasMore = hasMore, coursours = coursour, page = for_return.Cast<SongReturn>().ToList() };
            }
               

        }



        [Route("music/radio/add_to_history/{song_id}")]
        [HttpPost]
        public async Task<ActionResult> AddToHistory(Guid song_id)
        {


            return Ok();
        }
    



        [NonAction]
        public int Count( Albumn a)
        {
            return a.Songs.Sum(a => a.song_views.Count());
        }


       



      


        [Authorize]
        [AllowAnonymous]
        [Route("music/search/empty")]
        [HttpGet]
        public async Task<ActionResult> EmptySearchReturn()
        {
            var user = await _cms.ReturnUserModel(HttpContext.User);

            Console.WriteLine();
         

            List<AutoIncrementedParent> UserLastSearched(string user_id)
            {
             return    _context.songlastSearches.Where(a => a.UserId == user_id).OrderBy(a => a.last_searched).Take(10).AsQueryable().Union<LastSearchParent>(_context.albumnlastSearches.Where(a => a.UserId == user_id).OrderBy(a => a.last_searched).Take(10)).
                    Union<LastSearchParent>(_context.musicianlastSearches.Where(a=>a.UserId==user_id).OrderBy(a=>a.last_searched).Take(10)).OrderBy(a=>a.last_searched).Take(10)
                    .Select<LastSearchParent, AutoIncrementedParent>(a=>a.GetType().Name=="MusicianLastSearch" ?  ((MusicianLastSearch)a).Author : a.GetType().Name=="AlbumnLastSearch"? ((AlbumnLastSearch)a).Albumn
                    : ((SongLastSearch)a).Song ).ToList();   

            }
            
                   
//.Include(a=>a.Main_Author).
          
               if (user==null)
               {
                    return Ok(_cms.SongstoSongReturns(_context.songs.OrderBy(a=>a.song_views.Count()).ThenBy(a=>a.liked_by.Count()).Take(10).ToList(), null));
              }
              else
                {
 return Ok(_ms.AutoIncrementParentReturn(UserLastSearched(user.Id)));
                }
                      
        }

      [AllowAnonymous]
        [Authorize]
      [Route("music/search/albumn/songs/{id}")]
      [HttpGet]
      public async Task<ActionResult> AlbumnSongsReturn(Guid Id){
            var user = await _cms.ReturnUserModel(HttpContext.User);
            var albumn = _context.albumns.Find(Id);
            var songs = new List<Song>();
            
            var for_return = _cms.SongstoSongReturns(albumn.Songs.ToList(), user);
            Console.WriteLine("cringe");
            Console.WriteLine(for_return.Count);
        
          return Ok(for_return);
      }

      


        [Route("music/find_tag/{name}/{page_index}")]
        [HttpGet]
        public async Task<ActionResult> FindTag(string name, int page_index)
        {
            var for_return = new List< string>();
            var hasMore = true;

            var result = new List<Tags>();
            if (name=="") {
               result = _context.tags.OrderBy(a => a.song.Count()).Take(10).ToList();
            } else {
                result = _context.tags.Where(a => a.Name.Contains(name)).OrderBy(a => a.song.Count()).Skip(page_index * 10).Take(10).ToList();
            }
                foreach (var author in result)
                {
                    for_return.Add(author.Name);
                }
            if (for_return.Count <= 5||name=="")
            {
                hasMore = false;
            }
            return Ok(new PaginationreturnData(hasMore, for_return));
        }

        [Route("music/find_genre/{name}/{page_index}")]
        [HttpGet]
        public async Task<ActionResult> FindGenre(string name, int page_index)
        {
            var for_return = new List<string>();
            var hasMore = true;
            var result = new List<Genre>();
            if (name=="") {
                _context.genres.OrderBy(a => a.song.Count()).Take(10).ToList();
            } else { _context.musicians.Where(a => a.Name.Contains(name)).OrderBy(a => a.Songs.Count).Skip(page_index * 10).Take(10).ToList(); }
                foreach (var author in result)
                {
                    for_return.Add(author.Name);
                }
            if (for_return.Count <= 5 || name == "")
            {
                hasMore = false;
            }
            return Ok(new PaginationreturnData(hasMore, for_return));
        }



    }
}
