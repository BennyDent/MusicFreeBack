
using Cottle;
using EF6TempTableKit;
using EF6TempTableKit.DbContext;
using EF6TempTableKit.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using MusicFree.Migrations;
using MusicFree.Models;
using MusicFree.Models.DataReturnModel;
using MusicFree.Models.GenreAndName;
using MusicFree.Models.TempModels;
using MusicFree.Services;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Bcpg;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace MusicFree.Controllers
{
    [ApiController]
    public class RecomendationController : Controller
    {
        private readonly TempContext _tempContext;
        private readonly Random _rnd;
        private readonly UserContext _user_context;
        private readonly FreeMusicContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MusicService _ms;
        private readonly ContextMusicService _cms;
        public RecomendationController(UserManager<IdentityUser> userManager, FreeMusicContext context)
        {
            _tempContext = new TempContext();
            _userManager = userManager;
            _context = context;
            _rnd = new Random();
            _ms = new MusicService();
            _cms = new ContextMusicService(_userManager, _context);
        }







      




        [Route("")]
        [HttpPost("/similar/authors/{id}")]
        public async Task<ActionResult> SimilarAuthors(Musician author, Boolean is_popular)
        {


            return Ok();
        }








        enum Modes : int
        {
            tag,
            radio,
            albumn,
        }

        [NonAction]
        public int Randomizer()
        {


            return _rnd.Next(1, 100);






        }








        [NonAction]
        public int ChooseCompare(bool similar)
        {
            if (similar) return _cms.loose_compare; else return _cms.strict_compare;

        }


        

        [NonAction]
        public async Task<List<Song>> CompareFunction(int next_index, User user, Musician author)
        {



            var musicians = _cms.GetSimilarAuthors(author, 50);

            var tags = _cms.MusicianTags(author);


            bool MusicianCompareFunction(Song a, bool is_similar)
            {

                return musicians.Intersect(_ms.SongtoAuthors(a)).Any() || _cms.GenreCompare(SongtoGenres(a), _cms.MusicianToGenre(author), is_similar, ChooseCompare(is_similar))
                && _cms.TagCompare(SongtoTags(a), tags, is_similar, ChooseCompare(is_similar));
            }





       
        


            return CompareSongs(MusicianCompareFunction, next_index, 5);
        }

        private bool isInStack(Guid song_id,string user_id) {

            return _tempContext.stack.Where(a=> song_id==a.song_id&&a.user_id==user_id).Any();         
        }



        [NonAction]
        public List<Tags> CollectionSongtoTag(ICollection<Song> song_list)
        {

            var return_list = new List<Tags>();
            foreach (var song in song_list)
            {

                return_list.Concat(SongtoTags(song));


            }
            return_list = return_list.Distinct().ToList();
            return return_list;

        }

        [NonAction]
        public List<Tags> SongtoTags(Song song)
        {
            var for_return = new List<Tags>();
            foreach (var tags_list in song.tags)
            {
                for_return.Add(tags_list.tag);
            }
            return for_return;
        }

        [Authorize]
        [Route("author/popular_songs/{id}")]
        public async Task<ActionResult> MusicianFamousSongs(Guid id)
        {
            var songs = _context.songs.Where(a => a.Main_Author.Id == id || a.extra_authors.Where(a => a.AuthorId == id).Any()).OrderBy(a => a.song_views.Count)
                .ThenBy(a => a.liked_by.Count).Take(10).ToList();


            return Ok(_cms.SongstoSongReturns(songs, await  _cms.ReturnUserModel(HttpContext.User)));
        }



        [NonAction]
        public List<Genre> SongtoGenres(Song song)
        {
            var for_return = new List<Genre>();
            foreach (var tags_list in song.genres)
            {
                for_return.Add(tags_list.genre);
            }
            return for_return;
        }

        [Authorize(Roles = "User")]
        [Route("music/liked/last/albumns/{pages_index}/{page_size}/")]
        [HttpGet]
        public async Task<ActionResult> LikedLastAlbumns(int pages_index, int page_size)
        { var isMore = true;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var albumns = _context.albumn_views.Where(a => a.albumn.liked_by.Where(b => b.UserId == user.Id).Any()).OrderBy(a => a.last_listened)
                .Skip(page_size * pages_index).Take(page_size).Select(a => a.albumn).ToList();
            if (albumns.Count <= page_size)
            {
                isMore = false;
            }
            var result = new List<AlbumnReturn>();
            foreach (var albumn in albumns)
            {
                result.Add(new AlbumnReturn(albumn));
            }

            return Ok(new { isMore = isMore, page = result });
        }


        [NonAction]
        public List<Genre> CollectionSongtoGenre(ICollection<Song> song_list)
        {

            var return_list = new List<Genre>();
            foreach (var song in song_list)
            {


                return_list.Concat(SongtoGenres(song));
            }

            return_list = return_list.Distinct().ToList();
            return return_list;

        }


        

        [NonAction]
        public async Task AddStack(string user_id, Guid song)
        {

            if (_tempContext.stack.Where(a=>a.song_id== song&& a.user_id==user_id).Any())
            {

                _tempContext.stack.Where(a => a.song_id == song && a.user_id == user_id).First().timestamp = DateTime.Now;

            }
            else
            {
                var query = _tempContext.stack.Select(a => new StackModelDTO(a));
                var result = _tempContext.WithTempTableExpression<TempContext>(query).stack.Add(new StackModelDTO(new StackModel(song, user_id)));
            }

            await _tempContext.SaveChangesAsync();

        }


       


        [NonAction]
        public async Task<List<Song>> CompareFunction(int next_index, User user, Albumn albumn)
        {



            Guid id = albumn.Main_Author.Id;



            bool CompareFunction(Song a, bool is_similar) {
                return (a.Main_Author.Id == id && _rnd.Next(1, 100) < 40 || _cms.TagCompare(SongtoTags(a),
                _cms.AlbumnToTag(a.Albumn), is_similar, ChooseCompare(is_similar)) && _cms.GenreCompare(SongtoGenres(a), 
                _cms.AlbumnToGenre(a.Albumn), is_similar, ChooseCompare(is_similar)) &&
              !isInStack(a.Id, user.Id));

            }


            return CompareSongs(CompareFunction, next_index, _rnd.Next(1, 5));
        }
        [NonAction]
        public Boolean AuthorPossiibility(User user)
        {
            var rand = new Random();

            if (rand.Next(1, 100) < user.radio.same_author_possibility)
            {
                user.radio.same_author_possibility = user.radio.same_author_possibility - 20;
                if (user.radio.same_author_possibility < 0)
                {
                    user.radio.same_author_possibility = 60;
                }
                _user_context.SaveChanges();
                return true;
            }
            return false;
        }


        [NonAction]
        public List<Albumn> ReturnUserAlbumns(User user)
        {
            return _context.albumn_views.Where(a => a.UserId == user.Id).OrderBy(a => a.listened)
                .ThenBy(a => a.last_listened).Take(10).Select(a=>a.albumn).ToList();
        }


        [NonAction]
        public List<Tags> UserTags(User user)
        {

            return _cms.CollectionAlbumntoTags(ReturnUserAlbumns(user));
        }


        [NonAction]
        public List<Genre> UserGenre(User user)
        {
            return _cms.CollectionAlbumntoGenres(ReturnUserAlbumns(user));
        }

        [NonAction]
        public List<Song> MoreNextIndex(Func<Song, Boolean, Boolean> compare, int next, int similar, bool is_similar)
        {
            var songs = new List<Song>();

            songs = NextIndex(a => compare(a, is_similar), next - similar);

            songs = NextIndex(a => compare(a, is_similar), similar);

            return songs;
        }


        [NonAction]
        public List<Song> CompareSongs(Func<Song, bool, bool> compare, int next_index, int max_similar_index)
        {
            var new_song = new List<Song>();

            var similar_index = _rnd.Next(1, max_similar_index);

            if (next_index > similar_index)
            {
                new_song = new_song.Concat(MoreNextIndex(compare, next_index, similar_index, false)).ToList();

            }


            if (new_song.Count == 0)
            {
                new_song = new_song.Concat(NextIndex(a => compare(a, true), next_index)).ToList();


            }
            return new_song;
        }


        [NonAction]
        public List<Song> LessNextIndex(Func<Song, Boolean, Boolean> compare, int next)
        {

            Boolean is_similar() {
                var result = Randomizer();
                if (result < 40) {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            return NextIndex(a => compare(a, is_similar()), next);
        }

        [NonAction]
        public List<Song> NextIndex(Func<Song, Boolean> compare, int next)
        {

            return _context.songs.Where(compare).OrderBy(x => Randomizer()).Take(next).ToList();
        }



        [NonAction]
        public async Task<List<Song>> CompareFunction(int next_index, User user)
        {






            var new_song = new List<Song>();

            var tags = UserTags(user);
            tags = tags.Distinct().ToList();
            var genres = _context.genreUsers.Where(a => a.user_id == user.Id).OrderBy(a => a.listened).ThenBy(a => a.last_listened).Select(a => a.genre).ToList();

            var favorite_authors = _context.musician_likes.Where(a => a.UserId == user.Id).Select(a => a.author.Id).ToList();



            Boolean FirstCompare(Song a, bool is_similar)
            {
                return (favorite_authors.Contains(a.Main_Author.Id) && AuthorPossiibility(user) ||
                             _cms.TagCompare(CollectionSongtoTag([a]), tags, is_similar, ChooseCompare(is_similar)) || _cms.GenreCompare(SongtoGenres(a), genres, false, ChooseCompare(is_similar))) &&
                             !isInStack(a.Id, user.Id);
            }



            return CompareSongs(FirstCompare, next_index, _rnd.Next(1, 5));
        }


        [NonAction]
        public List<GenreTag> StringArrayToObjects(List<string> string_array, bool is_tag)
        {
            var result_array = new List<GenreTag>();


            foreach (var tag in string_array)
            { if (is_tag)
                {




                    var new_tag = _context.tags.Find(tag);
                    if (new_tag != null)
                    {
                        result_array.Add(new_tag);
                    }

                }
                else
                {

                    var new_tag = _context.genres.Find(tag);
                    if (new_tag != null)
                    {
                        result_array.Add(new_tag);
                    }

                }


            }


            return result_array;




        }





        //[Authorized]
      


        [NonAction]
        public async Task<List<Song>> CompareFunction(int next_index, User user, List<string>? tag_list, List<string>? genre_list)
        {

            var similar_index = _rnd.Next(1, 5);

            var tags_array = StringArrayToObjects(tag_list, true).ConvertAll(a => (Tags)a);
            var genres_array = StringArrayToObjects(genre_list, false).ConvertAll(a => (Genre)a);

            Boolean SimilarCompare(Song a, bool is_similar)
            {
                return (_cms.TagCompare(SongtoTags(a), tags_array, is_similar, ChooseCompare(is_similar)) && _cms.GenreCompare(SongtoGenres(a), genres_array, is_similar, ChooseCompare(is_similar))) &&
                                AuthorPossiibility(user);

            }

            Boolean TagOnlyCompare(Song a, bool is_similar)
            {
                return _cms.TagCompare(SongtoTags(a), tags_array, is_similar, ChooseCompare(is_similar)) |
            AuthorPossiibility(user);
            }
            bool GenreOnlyCompare(Song a, bool is_similar)
            {
                return _cms.GenreCompare(SongtoGenres(a), genres_array, is_similar, ChooseCompare(is_similar)) |
           AuthorPossiibility(user);
            }
            if (tag_list != null && genre_list != null)
            {

                return CompareSongs(SimilarCompare, next_index, 5);
            }

            if (tag_list != null && genre_list == null)
            {
                return CompareSongs(TagOnlyCompare, next_index, 5);
            }
            else
            {
                return CompareSongs(GenreOnlyCompare, next_index, 5);
            }

        }


      



        [Route("")]
        [HttpGet("/radio/{type}/next/{next_index}/")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/{id2}")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/{id2}/{id3}")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/{id2}/{id3}/{id4}")]
        [HttpGet]
        public async Task<ActionResult> MainRadioNext(string type, int next_index,string? id,  string? id2, string? id3, string? id4)
        { var user = await _cms.ReturnUserModel(HttpContext.User);
            var ms = new MusicService();
            var rnd = new Random();

            List<Song> songs = new List<Song>();
            try {

                if (type == "albumn")
                {
                    var albumn = await _context.albumns.FindAsync(new Guid(id));

                    songs = await CompareFunction(next_index, user, albumn);
                }
                else if (type == "author")
                {
                    var author = await _context.musicians.FindAsync(new Guid(id));
                    songs = await CompareFunction(next_index, user, author);
                }
                else
                {

                    if (id == null)
                    {

                        songs = await CompareFunction(next_index, user);
                    }
                    var tags = new List<string> { id, id2, id3, id4 };

                    if (type == "tag")
                    {
                        songs = await CompareFunction(next_index, user, tags, null);
                    }
                    else
                    {
                        songs = await CompareFunction(next_index, user, null, tags);
                    }


                }


                if (next_index == 1)
                {
                    user.radio.radio_index += 1;
                }
                else if (next_index == 10) {

                }


                foreach (var song in songs)
                {
                   await AddStack(user.Id, song.Id);
                }

                return Ok(_cms.SongstoSongReturns(songs, user));

            }
            catch
            {
                return BadRequest();
            }

        }

        [Authorize]
        [Route("music/search/back/history/{song_id}")]
        [HttpGet]
        public async Task<ActionResult> GetBackHistory(Guid song_id)
        {
            var song_listened = _tempContext.stack.Where(a=>a.song_id==song_id).First();
            var result = _tempContext.stack.Where(a=> a.song_id==song_id&& DateTime.Compare(song_listened.timestamp, a.timestamp)<0).OrderByDescending(a =>a.timestamp).First();

            return Ok(_cms.SongtoSongReturn(await _context.songs.FindAsync(result.song_id),await _cms.ReturnUserModel(HttpContext.User)));


          
        }



        [Authorize]
        [Route("recommendation/albumns/listened")]
        [HttpGet]
        public async Task<ActionResult> AlbumnsStartedListen()
        {
            var user = await _userManager.GetUserAsync(User);
            var albumns = _context.albumns.Where(a => a.Songs.Where(a => a.song_views.Where(b => b.UserId == user.Id).Any()).ToList().Count > 1)
                .OrderBy(a => a.albumn_views.Where(b => b.UserId == user.Id).First().last_listened).ToList();
            return Ok(_ms.AlbumnstoAlbumnReturn(albumns));
        }






     





       


       



        [NonAction]
        public async Task<List<Albumn>> RecentAlbumnReturn(bool is_public,User? user )
        {
            var result = new List<Albumn>();
          
          
                if (is_public)
                {
                result = _context.albumns.OrderBy(a=> a.release_date).ThenBy(a=>a.albumn_views.Count).ThenBy(a=>a.liked_by.Count).Take(15).ToList();
            }
            else
            {if(user != null)
                {
 result = _context.albumns.Where(a=> _cms.AlbumnView(a,user) != null).OrderBy(a=>a.release_date)
                    .ThenBy(a=> _cms.AlbumnView(a, user).last_listened).ThenBy(a=> _cms.AlbumnView(a, user).listened).Take(15).ToList();
                }
               
            }


            return result;


           
        }


        [Authorize(Roles = "User, Guest")]
        [Route("recommendation/albumns/recent_releases/public")]
        [HttpGet]
    public async Task<ActionResult> PublicNewAlbumns()
        {
            

            return Ok(_ms.AlbumnstoAlbumnReturn(await RecentAlbumnReturn(true, null)));
        }


        [Authorize(Roles ="User")]
        [Route("recommendation/albumns/recent_releases/public")]
        [HttpGet]
        public async Task<ActionResult> PersonalNewAlbumns()
        {
            var user = await _cms.ReturnUserModel(HttpContext.User);
            return Ok(_ms.AlbumnstoAlbumnReturn(await RecentAlbumnReturn(false, user)));
        }
    
    }
    } 
