using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using MusicFree.Services;
using Microsoft.AspNetCore.WebUtilities;
using MusicFree.Models;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Options;
using ZstdSharp.Unsafe;
using MusicFree.utilities;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Authorization;
using Org.BouncyCastle.Utilities;
using Microsoft.AspNetCore.Authorization;
using MusicFree.Models.DataReturnModel;
namespace MusicFree.Controllers
{

    [ApiController]
    public class MusicFindController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly FreeMusicContext _context;
        public MusicFindController(FreeMusicContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [Route("search/author/{name}/{views_amount}")]
        [HttpGet("search/find_author/{name}/{views_amount}")]
        public async Task<ActionResult> AuthorFind(string name, int views_amount)
        {
            var ms = new MusicService();
            var nextPage = new List<Musician>();
            var find = _context.musicians.First();
            if (find.liked_by == null)
            {
                Console.WriteLine("why null");
            }
            if (views_amount == -5 || views_amount == -10)
            {
                Console.WriteLine(Math.Abs(views_amount));
//.OrderByDescending(a => a.liked_by.Count() )
                nextPage = _context.musicians.Where(a => a.Name.Contains(name)).Take(Math.Abs(views_amount)).ToList();
                
                foreach (var page in nextPage)
                {
                    Console.WriteLine(page.Name);
                }
            }
            else
            {
                nextPage = _context.musicians.Where(a => a.Name.Contains(name) & a.liked_by.Count() < views_amount).OrderByDescending(a => a.liked_by.Count()).Take(10).ToList();
            }
            var result = ms.MusicianToAuthorReturn(nextPage);
            if (nextPage.Count() == 0)
            {
                return Ok();
            }
            var newCoursor = -1;
          if(_context.musicians.Where(a => a.Name.Contains(name) & a.liked_by.Count() < nextPage.Last().liked_by.Count()).Any())
            {
                newCoursor = nextPage.Last().liked_by.Count();
            }
            return Ok(new {newCoursor=newCoursor, search=result });
        }



        [NonAction]
        public int Count( Albumn a)
        {
            return a.Songs.Sum(a => a.song_views.Count());
        }


        [Route("search/albumn/{name}/{views_amount}")]
        [HttpGet]
        public async Task<ActionResult> AlbumnFind(string name, int views_amount)
        {
            var ms = new MusicService();
            var nextPage = new List<Albumn>();


            _context.albumns.First().albumn_views = new List<AlbumnViews>();
            await _context.SaveChangesAsync();
            Console.WriteLine(_context.albumns.Where(a=> a.Name=="Luck").Include(a=>a.Main_Author).First().Main_Author.Name);

            Console.WriteLine("name");
            if (views_amount == -5 || views_amount == -10)
            {
                nextPage = _context.albumns.Where(a => a.Name.Contains(name)).AsEnumerable().OrderByDescending(a=>a.albumn_views.Count).Take(Math.Abs(views_amount)).ToList();
            }
            else
            {
                nextPage = _context.albumns.Where(a => a.Name.Contains(name) & a.albumn_views.Count < views_amount).AsEnumerable().OrderByDescending(a => a.albumn_views.Count).Take(10).ToList();
            }
            var newCursor = -1;
            if (nextPage.Count() == 0)
            {
                return Ok();
            }
            // 
            
            if (nextPage.Count()>0)
            {
                
               


                 if (_context.albumns.Where(a =>a.Name.Contains(name)).Where(a=>a.albumn_views.Count> nextPage.Last().albumn_views.Count).Any())
            {
                newCursor = ms.AlbumnViews(nextPage.Last());
            }
           
            }
          
            var array = new List<AlbumnRethurn>();

            foreach(var page in nextPage)
            {
                array.Add(ms.AlbumnReturn(page) );
            }

            return Ok(new {Coursor= newCursor, search=array });
        }
        [Route("search/song/{name}/{views_amount}")]
        [HttpGet("search/song/{name}/{views_amount}")]
        public async Task<ActionResult> SongFind(string name, int views_amount) {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var ms = new MusicService();
            var nextPage = new List<Song>();
            Console.WriteLine(333);
            if (views_amount == -5||views_amount==-10)
            {
                nextPage = _context.songs.Where(a => a.Name.Contains(name)).OrderByDescending(a => a.song_views.Count()).Take(Math.Abs(views_amount)).ToList();
            }
            else
            {


                nextPage = _context.songs.Where(a => a.Name.Contains(name) & a.song_views.Count() < views_amount).OrderByDescending(a => a.song_views.Count()).Take(10).ToList();
            }
            Console.WriteLine(131331);
            var newCursor = -1;
            Console.WriteLine(nextPage.Count());
            Console.WriteLine("why");
            if (nextPage.Count() == 0)
            {
                Console.WriteLine("here");
                return Ok();
            }
            Console.WriteLine("will you work?");

            /* var songs = _context.songs.ToList();
             foreach (var song in songs) {
                 song.song_views = new List<SongViews>();
             }*/
/*
          var songs = _context.songs.ToList();

          var author = _context.musicians.Where(a => a.Name == "Ilya").First();
          foreach (var song in songs) { 
          song.Main_Author= author;
          }*/


              if (_context.songs.Where(a=> a.Name.Contains(name) & a.song_views.Count <nextPage.Last().song_views.Count).Any())
          {
   newCursor = nextPage.Last().song_views.Count();
          }

           
          var result_array = new List<SongReturn>();
          foreach (var song in nextPage) {
                Console.WriteLine(song.Name);  
            if(song!= null) {

                    if (song.Main_Author == null)
                    { var AuthorFirst = _context.musicians.First();
                        song.Main_Author =AuthorFirst;
                        AuthorFirst.Songs.Add(song);
                        await _context.SaveChangesAsync();
                        
                    }
                    var new_song = ms.SongToSongReturn(song, user);
Console.WriteLine(77);
                        Console.WriteLine(new_song.is_liked);
 if(song.Albumn != null) {
                  new_song.albumn_id = song.Albumn.Id;
                        
              }
result_array.Add(new_song);
}
         

               }
          Console.WriteLine(result_array[0].Name);
          return Ok(new {coursor=newCursor, search= result_array });





      }

      [Route("search/songs/albumn/{id}")]
      [HttpGet]
      public async Task<ActionResult> AlbumnSongsReturn(Guid Id){
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var albumn = _context.albumns.Find(Id);
          var for_return = new List<SongReturn>();
          var ms = new MusicService();
          foreach(var song in albumn.Songs)
          {
              for_return.Add(ms.SongToSongReturn(song, user));
          }

          return Ok(for_return);
      }


      [Route("search/songs/author/{id}")]
      [HttpGet]
      public async Task<ActionResult> AuthorSongsReturn(Guid Id){
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var author = _context.musicians.Find(Id);
          var for_return= new List<SongReturn>();
          var ms = new MusicService();
          foreach (var song in author.Songs.OrderByDescending(a=>a.song_views.Count()).Take(10).ToList())
          {
              for_return.Add(ms.SongToSongReturn(song, user));
          }
          return Ok(for_return);
      }

[Route("music/find_author/{name}/{page_index}")]
          [HttpGet]
          public async Task<ActionResult> FindAuthor(string  name, int page_index){
          var for_return = new List<AuthorReturn>();
            var hasMore = true;
            foreach (var author in _context.musicians.Where(a => a.Name.Contains(name)).OrderBy(a=>a.Songs.Count).Take(page_index*5).ToList()) {
              for_return.Add(new AuthorReturn(author.Id, author.Name));
          }

            if (for_return.Count <= 5) {
                hasMore = false;
            } 

              return Ok(new PaginationreturnData(hasMore, for_return));
          }



  } 
  }
