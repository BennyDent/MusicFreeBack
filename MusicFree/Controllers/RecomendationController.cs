using Microsoft.AspNetCore.Mvc;
using MusicFree.Models;
using Microsoft.AspNetCore.Identity;
using HotChocolate.Authorization;
using MusicFree.Services;

using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using System.ComponentModel;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System.Runtime.InteropServices;
using System.Linq;
using HotChocolate.Language;
using MusicFree.Models.GenreAndName;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace MusicFree.Controllers
{
    [ApiController]
    public class RecomendationController : Controller
    {
        private readonly UserContext _user_context;
        private readonly FreeMusicContext _context;
        private readonly UserManager<User> _userManager;
        public RecomendationController(UserManager<User> userManager, FreeMusicContext context)
        {
            _userManager = userManager;
            _context = context;
        }

 [NonAction]
       public  List<Genre> CollectionToGenre(List<GenreCollection> input)
            {
                var  genres = new List<Genre>();    
                foreach (var genre in input)
                {
                    genres.Add(genre.genre);
                }
                return genres;
            }

        [NonAction]
        public List<Tags> CollectiontoTag(List<TagCollection> input)
        {
            var genres = new List<Tags>();
            foreach (var genre in input)
            {
                genres.Add(genre.tag);
            }
            return genres;

        }



        [NonAction]
        public Boolean GenreSimilar(Genre genre, Genre compare) {
            var is_similar = false;

            var genre_similar = new List<string>();
            foreach (var value in genre.similar)
            {   
                genre_similar.Add((genre.Name == value.first_id) ? value.first_id : value.second_id);
            }

            if ( genre_similar.Contains(compare.Name))
            {
                return true;
            }
            return false;

            }

        [NonAction]
        public Boolean ListGenreSimilar(List<Genre> author_genres, List<Genre> main_author_genres )
        {
          
            var counter = 1;
           
          
            foreach (var main_author_genre in main_author_genres)
            {

                foreach (var author_genre in author_genres)
                {
                    if (GenreSimilar(author_genre, main_author_genre))
                    {
                        if (counter == 1)
                        {
                            counter--;
                        }
                        else
                        {
                            return true;
                        }

                    }
                   
                }
             
            }
   return false;
        }

        [NonAction]

        public Boolean ListTagSimilar(List<Tags> author_genres, List<Tags> main_author_genres)
        {

            var counter = 1;


            foreach (var main_author_genre in main_author_genres)
            {

                foreach (var author_genre in author_genres)
                {
                    if (TagSimilar(author_genre, main_author_genre))
                    {
                        if (counter == 1)
                        {
                            counter--;
                        }
                        else
                        {
                            return true;
                        }

                    }

                }

            }
            return false;
        }


        [NonAction]
        public Boolean TagSimilar(Tags tag, Tags compare)
        {
            var is_similar = false;

            var genre_similar = new List<string>();
            foreach (var value in tag.similar)
            {
                genre_similar.Add((tag.Name == value.first_id) ? value.first_id : value.second_id);
            }

            if (genre_similar.Contains(compare.Name))
            {
                return true;
            }
            return false;
        }


       

        [NonAction]
        public Boolean GenreCompare(List<Genre> input_genre, List<Genre> compare_genre, Boolean is_similar)
        {
            var is_genre_similar = ListGenreSimilar(input_genre, compare_genre );

            if (input_genre.Intersect(compare_genre).Count() > 2| (is_similar && is_genre_similar))
            {
                return true;
            }
            else return false;
        }

        [NonAction]
        public Boolean TagCompare(List<Tags> input_tag, List<Tags> compare_tag, Boolean is_similar )
        {
            var is_tag_similar = ListTagSimilar(input_tag, compare_tag); 

            if (input_tag.Intersect(compare_tag).Count() > 2|(is_similar&& is_tag_similar))
            {
                return true;
            } else return false;
        }



      

        [NonAction]
        public List<Tags> CollectionSongtoTag(ICollection<Song> song_list)
        {
            
            var return_list = new List<Tags>();
            foreach (var song in song_list){

                return_list.Concat(SongtoTags(song));     
            
           
 }
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



        [NonAction]
        public List<Genre> CollectionSongtoGenre(ICollection<Song> song_list)
        {

            var return_list = new List<Genre>();
            foreach (var song in song_list)
            {


                return_list.Concat(SongtoGenres(song));
            }
            return return_list;

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

List<Genre> AuthorGenres(Musician author)
        {
            return CollectionSongtoGenre(author.Songs.OrderBy(a=> a.song_views.Count).Take(10).ToList());
        }

List<Tags> AuthorTags(Musician author)
            {
                return CollectionSongtoTag(author.Songs.OrderBy(a => a.song_views.Count).Take(10).ToList());
            }
           
            Boolean Similar(Musician a)
            {
                return GenreCompare(AuthorGenres(a), AuthorGenres(author), true)&& TagCompare(AuthorTags(a), AuthorTags(author), true);
            }

            Boolean Not_Similar(Musician a)
            {
                return GenreCompare(AuthorGenres(a), AuthorGenres(author), true) && TagCompare(AuthorTags(a), AuthorTags(author), false);
            }

            List<Musician> authors = null;
            List<Musician> similar_authors = null;
            if (is_popular) {

                authors = _context.musicians.Where(Not_Similar).OrderByDescending(a => a.liked_by.Count()).Take(5).ToList();
                similar_authors = _context.musicians.Where(Similar).OrderByDescending(a => a.liked_by.Count()).Take(2).ToList();
            } else {
                authors = _context.musicians.Where(Not_Similar).OrderBy(a => a.liked_by.Count()).Take(5).ToList();
                similar_authors = _context.musicians.Where(Similar).OrderBy(a => a.liked_by.Count()).Take(2).ToList();
            }
            var return_result = new List<Object>();
            foreach (var Author in authors) {
                return_result.Add(new { name = Author.Name, id = Author.Id, src = Author.img_filename, });
            }
            foreach (var Author in similar_authors)
            {
                return_result.Add(new { name = Author.Name, id = Author.Id, src = Author.img_filename, });
            }
            return Ok(return_result);
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

            var rand = new Random();
           int result = rand.Next(1, 100);

            return result;


            
            
        }

       

        [NonAction]
        public Boolean UserCompare(User user, Song song)
        {
            return !(user.radio.radio_stack.Contains(song.Id) && user.radio.radio_stack.Contains(song.Id));
        }


        [NonAction]
        public async Task<List<Song>> CompareFunction(int next_index, User user, Albumn albumn )
        {
            var rnd = new Random();

            var new_song = new List<Song>();


            Guid id = albumn.Main_Author.Id;
            new_song = _context.songs.Where(a => (a.Main_Author.Id == id && rnd.Next(1, 100) < 40 ||MusicianGenreCompare(a, albumn, true, false) || ParentTagCompare(a, albumn)) &&
            UserCompare(user, a)).OrderBy(x => rnd.Next()).Take(next_index).ToList();
            if (new_song.Count == 0) {
                new_song = _context.songs.Where(a => (a.Main_Author.Id == id && rnd.Next(1, 100) < 40 || MusicianGenreCompare(a, albumn, true, false) || ParentTagCompare(a, albumn))).Take(next_index).ToList();

               
            } 
           
         return new_song;
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
        public async Task<List<Song>> CompareFunction(int next_index, User user, Albumn albumn)
        {
            var rnd = new Random();

            var new_song = new List<Song>();

            var tags = _context.tag_user.Where(a=> a.user_id==user.Id).OrderBy(x=> x.listened)
                .ThenBy(a=> a.last_listened).Take(10).Select(a=> a.tag).ToList();

            var genre = _context.genre_user.Where(a => a.user_id == user.Id ).OrderBy(x => x.listened)
                .ThenBy(a => a.last_listened).Take(10).Select(a => a.genre).ToList();

            var favorite_authors = _context.songsViews.Where(a => a.UserId==user.Id ).OrderBy(x => x.listened).ThenBy(a => a.last_listened).Take(10)
                .Select(a=>a.song.Main_Author).ToList();


            new_song = _context.songs.Where(a => ( favorite_authors.Contains(a.Main_Author)&&AuthorPossiibility(user)|| 
            TagCompare(CollectionSongtoTag([a]),tags, true )|| GenreCompare(SongtoGenres(a), genre, false)) &&
            UserCompare(user, a)).OrderBy(x => rnd.Next()).Take(next_index).ToList();
            if (new_song.Count == 0)
            {
                new_song = _context.songs.Where(a => (favorite_authors.Contains(a.Main_Author) && AuthorPossiibility(user) ||
            TagCompare(Song, tags) || GenreCompare(SongtoGenres(a)), genre)) &&
            UserCompare(user, a)).OrderBy(x => rnd.Next()).Take(next_index).ToList();


            }
            return new_song;
        }


        [NonAction]
        public async Task<List<GenreTag>> StringArrayToObjects(List<string> string_array, bool is_tag)
        {
           var result_array = new List<GenreTag>();

            
            foreach (var tag in string_array)
            {  if (is_tag)
                {



                   
                    var new_tag = await _context.tags.FindAsync(tag);
                    if (new_tag != null)
                    {
                       result_array.Add(new_tag);
                    }

                }
                else
                {

                   var new_tag = await _context.genres.FindAsync(tag);
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
                var rnd = new Random();

                var new_song = new List<Song>();
           
                if (tag_list != null && genre_list !=null)
            {
                var tags_array = await StringArrayToObjects(tag_list, true);
                var genres_array = await StringArrayToObjects(genre_list, false);
                new_song = _context.songs.Where(a => (TagCompare(SongToTags(a), tags_array)&& GenreCompare(SongToGenres(a), genres_array)) &&
                AuthorPossiibility(user)).OrderBy(x => rnd.Next()).Take(next_index).ToList();
               if(new_song.Count == 0)
                {
                    new_song = _context.songs.Where(a => (TagCompare(CollectionSongtoTag(a.tags), tags_array)) &&
                AuthorPossiibility(user)).OrderBy(x => rnd.Next()).Take(next_index).ToList();
                }
            }

            if(tag_list != null && genre_list == null)
            {
                var tags_array = await StringArrayToObjects(tag_list, true);
                new_song = _context.songs.Where(a => (TagCompare(CollectionSongtoTag(a.tags), tags_array)) && AuthorPossiibility(user))
                    .OrderBy(x => rnd.Next()).Take(next_index).ToList();
                if (new_song.Count == 0)
                {
                    new_song = _context.songs.Where(a => (TagCompare(CollectionSongtoTag(a.tags), tags_array)))
                     .OrderBy(x => rnd.Next()).Take(next_index).ToList();
                }
            }
            else
            {
                var genres_array = await StringArrayToObjects(genre_list, false);
                new_song = _context.songs.Where(a => (TagCompare(CollectionSongtoTag(a.tags), genres_array)) &&
                 AuthorPossiibility(user)).OrderBy(x => rnd.Next()).Take(next_index).ToList();
                if (new_song.Count == 0)
                {
                    new_song = _context.songs.Where(a => (TagCompare(CollectionSongtoTag(a.tags), genres_array)))
                        .OrderBy(x => rnd.Next()).Take(next_index).ToList();
                }
            }
      return new_song;
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
            Albumn albumn;
            List<Song> songs = new List<Song>();
            if(type == "albumn")
            {
                albumn = await _context.albumns.FindAsync(new Guid(id));

                songs = await CompareFunction(next_index, user, albumn);
            }
            else
            {

if (id==null) { 
                
                songs = await CompareFunction(next_index, user);
                }
 var tags = new List<string>{id,id2,id3, id4 };

if (type=="tag") {
                    songs = await CompareFunction(next_index, user,tags, null);
                }
                else
                {
                    songs = await CompareFunction(next_index, user,null, tags );
                }
                
                
            }


            if (next_index==1)
            {
                user.radio.radio_index += 1;
            }
            else if(next_index==10) {
            
            }

            return Ok( ms.SongstoSongReturns(songs, user) ); 
     
        
        
        }

 

        


      

    }
    } 
