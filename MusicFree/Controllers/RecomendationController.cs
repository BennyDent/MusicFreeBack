
using HotChocolate.Language;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using MusicFree.Models;
using MusicFree.Models.GenreAndName;
using MusicFree.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;

namespace MusicFree.Controllers
{
    [ApiController]
    public class RecomendationController : Controller
    {

        private readonly Random _rnd;
        private readonly UserContext _user_context;
        private readonly FreeMusicContext _context;
        private readonly UserManager<User> _userManager;
        private readonly MusicService _ms;
        private readonly ContextMusicService _cms;
        public RecomendationController(UserManager<User> userManager, FreeMusicContext context)
        {
            _userManager = userManager;
            _context = context;
            _rnd = new Random();
            _ms = new MusicService();
            _cms = new ContextMusicService(_userManager, _context, _user_context);
        }

 



      

       [ Route("")]
        [HttpGet("radio/get_from_history/{song_id}/{number}")]
     public async Task<ActionResult> GetFromHistory(Guid id, int number)
        {
            var result_array = new Song[5];
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = user.radio.radio_stack.Where(a=> a==id).First();
            var index =  user.radio.radio_stack.ToArray().ToList().IndexOf(result);
            if(index < number)
            {
               
                for(int a= 0, b= 0; a <5; a++)
                {
                    if(index != 0)
                    {
                        result_array.Push(await _context.songs.FindAsync( user.radio.radio_stack.ToArray()[index-a]));
                    }
                    else
                    {

                        result_array.Push(await _context.songs.FindAsync(user.radio.radio_history[b]));
                        b++;
                    }
                }
            }
            else
            {
                for (int a = 0, b = 0; a < 5; a++)
                {
                    result_array.Push(await _context.songs.FindAsync(user.radio.radio_stack.ToArray()[index - a]));
                }
                }
            return Ok(result_array);
        }

       

      


        [Route("")]
        [HttpPost("/similar/authors/{id}")]
        public async Task<ActionResult> SimilarAuthors(Musician author, Boolean is_popular)
        {


            return Ok();
        } 

     




       

        enum Modes: int
        { 
            tag,
            radio ,
            albumn ,
        }

        [NonAction]
        public int Randomizer()
        {

           
            return  _rnd.Next(1, 100);

          


            
            
        }


        [NonAction]
        public  int ChooseCompare(bool similar)
        {
            if (similar) return _cms.loose_compare;else return _cms.strict_compare;
            
        }


        [NonAction]
        public Boolean UserCompare(User user, Song song)
        {
            return !(user.radio.radio_stack.Contains(song.Id) && user.radio.radio_stack.Contains(song.Id));
        }

        [NonAction]
        public async Task<List<Song>> CompareFunction(int next_index, User user, Musician author)
        {

          

            var musicians = _cms.GetSimilarAuthors(author,50);

            var tags = _cms.MusicianTags(author);


            bool MusicianCompareFunction(Song a, bool is_similar)
            {

                return musicians.Intersect(_ms.SongtoAuthors(a)).Any() ||_cms.GenreCompare(SongtoGenres(a),_cms.MusicianToGenre(author), is_similar, ChooseCompare(is_similar))
                && _cms.TagCompare(SongtoTags(a), tags, is_similar, ChooseCompare(is_similar)) ;
            }




            return  CompareSongs(MusicianCompareFunction, next_index,5);
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
           
            
            return Ok(_cms.SongstoSongReturns(songs, await _userManager.GetUserAsync(HttpContext.User)));
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
        public async  Task<ActionResult> LikedLastAlbumns(int pages_index, int page_size )
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var albumns = _context.albumns.Where(a=> a.albumn_views.Where(a=> a.UserId==user.Id).Any()).OrderBy().Take();

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
        public async Task<List<Song>> CompareFunction(int next_index, User user, Albumn albumn )
        {
            

           
            Guid id = albumn.Main_Author.Id;

           

            bool CompareFunction(Song a, bool is_similar) { 
            return (a.Main_Author.Id == id && _rnd.Next(1, 100) < 40 || _cms.TagCompare(SongtoTags(a),
            _cms.AlbumnToTag(a.Albumn), is_similar, ChooseCompare(is_similar) ) && _cms.GenreCompare(SongtoGenres(a), _cms.AlbumnToGenre(a.Albumn), is_similar, ChooseCompare(is_similar))) &&
            UserCompare(user, a);
            
            }
           
           
         return CompareSongs(CompareFunction, next_index, _rnd.Next(1,5));
        }
        [NonAction]
        public Boolean AuthorPossiibility(User user)
        {
            var rand = new Random();
            
            if(rand.Next(1, 100)< user.radio.same_author_possibility)
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
        public List<Tags> UserTags(User user)
        {
            var rnd = new Random();
             var tags = new List<Tags>();

            DateTime MinTagDateTime(Tags a)
            {
               return a.song.Where(a=> a.song.song_views.Where(b=>b.UserId==user.Id).Any())
                    .OrderBy(a => a.song.song_views.OrderBy(a => a.last_listened).First()).First().song.song_views
                    .Where(a => a.song.song_views.Where(b => b.UserId == user.Id).Any()).First().last_listened;
                
            }


            void GetUserTags()
            {
                tags = tags.Concat(_context.tags.Where(a => a.song.Where(b => _cms.SongListened(b.song, user) > 5)
                .Any() && !tags.Contains(a)).Distinct().OrderBy(MinTagDateTime)
                .ThenBy(a => a.song.Where(b => _cms.SongListened(b.song, user) > 5)
                .ToList().Count).Take(25).ToList()).Concat(_context.tags.Where(a => a.song.Where(b => _cms.isSongLiked(b.song, user))
                .Any() && !tags.Contains(a)).Distinct().OrderBy(MinTagDateTime)
                .ThenBy(a => a.song.Where(b => _cms.SongListened(b.song, user) > 5).ToList().Count).Take(25).ToList()).Distinct().ToList();

            }




            while (tags.Count < 20)
            {
                GetUserTags();
            }
           
            
            return tags;
        }


        [NonAction]
        public List<Song> MoreNextIndex( Func<Song, Boolean, Boolean> compare, int next, int similar, bool is_similar)
        {
            var songs = new List<Song>();

            songs = NextIndex(a=> compare(a, is_similar),next-similar);

            songs = NextIndex(a=> compare(a, is_similar), similar);

            return songs;
        }


        [NonAction]
        public List<Song> CompareSongs(Func<Song, bool, bool> compare,int next_index, int max_similar_index)
        {
            var new_song = new List<Song>();

            var similar_index = _rnd.Next(1, max_similar_index);

            if (next_index > similar_index)
            {
               new_song =  new_song.Concat(MoreNextIndex(compare, next_index, similar_index, false)).ToList();

            }

            
            if (new_song.Count == 0)
            {
                new_song = new_song.Concat(NextIndex(a=>compare(a, true), next_index)).ToList();  


            }
            return new_song;
        }


        [NonAction]
        public List<Song> LessNextIndex(Func<Song,Boolean, Boolean> compare, int next)
        {
          
           Boolean is_similar() {
                var result = Randomizer();
                if (result< 40) {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            return NextIndex(a=> compare(a, is_similar()), next);
        }

        [NonAction]
        public List<Song> NextIndex(Func<Song, Boolean> compare, int next)
        {

  return  _context.songs.Where(compare).OrderBy(x => Randomizer()).Take(next).ToList();
        }



        [NonAction]
        public async Task<List<Song>> CompareFunction(int next_index, User user)
        {

           
            
            
            
            
            var new_song = new List<Song>();

            var tags = UserTags(user);
            tags = tags.Distinct().ToList();
            var genres = _context.genreUsers.Where(a=>a.user_id==user.Id).OrderBy(a=>a.listened).ThenBy(a=>a.last_listened).Select(a=> a.genre).ToList();

            var favorite_authors = _context.musician_likes.Where(a => a.UsersId == user.Id).Select(a=> a.author.Id).ToList();

           

            Boolean FirstCompare(Song a, bool is_similar )
            {
               return (favorite_authors.Contains(a.Main_Author.Id) && AuthorPossiibility(user) ||
                            _cms.TagCompare(CollectionSongtoTag([a]), tags, is_similar, ChooseCompare(is_similar) )|| _cms.GenreCompare(SongtoGenres(a), genres, false, ChooseCompare(is_similar))) &&
                            UserCompare(user, a);
            }



            return CompareSongs(FirstCompare, next_index, _rnd.Next(1, 5));
        }


        [NonAction]
        public List<GenreTag> StringArrayToObjects(List<string> string_array, bool is_tag)
        {
           var result_array = new List<GenreTag>();

            
            foreach (var tag in string_array)
            {  if (is_tag)
                {



                   
                    var new_tag = _context.tags.Find(tag);
                    if (new_tag != null)
                    {
                       result_array.Add(new_tag);
                    }

                }
                else
                {

                   var new_tag =  _context.genres.Find(tag);
                    if (new_tag != null)
                    {
                        result_array.Add(new_tag);
                    }

                }
                

            }


            return result_array;
            



        }


      


      

        [NonAction]
        public async Task<List<Song>>  CompareFunction(   int next_index, User user, List<string>? tag_list, List<string>? genre_list  )
        {
          
            var similar_index = _rnd.Next(1, 5);
            
            var tags_array = StringArrayToObjects(tag_list, true).ConvertAll(a => (Tags)a);
            var genres_array = StringArrayToObjects(genre_list, false).ConvertAll(a => (Genre)a);
            
            Boolean SimilarCompare(Song a, bool is_similar)
            {
                return (_cms.TagCompare(SongtoTags(a), tags_array, is_similar, ChooseCompare(is_similar)) && _cms.GenreCompare(SongtoGenres(a), genres_array, is_similar, ChooseCompare(is_similar))) &&
                                AuthorPossiibility(user);

            }

            Boolean TagOnlyCompare(Song a,  bool is_similar)
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

              return   CompareSongs(SimilarCompare,next_index, 5);
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
        [HttpPatch("/radio/reset")]
        public async  Task<ActionResult> RadioReset(string id, Boolean is_popular)
        {
           var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                user.radio.radio_index = 0;
                await _user_context.SaveChangesAsync();
            }
            return BadRequest();
        }




        [Route("")]
        [HttpGet("/radio/{type}/next/{next_index}/")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/{id2}")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/{id2}/{id3}")]
        [HttpGet("/radio/{type}/next/{next_index}/{id}/{id2}/{id3}/{id4}")]
        [HttpGet]
        public async Task<ActionResult> MainRadioNext(string? id, string type, int next_index, string? id2, string? id3, string? id4)
        { var user = await _userManager.GetUserAsync(HttpContext.User);
            var ms = new MusicService();
        var rnd = new Random();
            
            List<Song> songs = new List<Song>();
            try { 
            
            if(type == "albumn")
            {
               var albumn = await _context.albumns.FindAsync(new Guid(id));

                songs = await CompareFunction(next_index, user, albumn);
            }
            else if(type == "author")
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


            if (next_index==1)
            {
                user.radio.radio_index += 1;
            }
            else if(next_index==10) {
            
            }



            return Ok( _cms.SongstoSongReturns(songs, user) ); 
     
        }
            catch
            {
                return BadRequest();
            }
        
        }

 

        


      

    }
    } 
