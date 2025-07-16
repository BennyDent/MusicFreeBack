using Microsoft.AspNetCore.Mvc;
using System.Web;
using MusicFree.Services;
using Microsoft.AspNetCore.WebUtilities;
using MusicFree.Models;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using ZstdSharp.Unsafe;
using MusicFree.utilities;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Data;
using SharpCompress.Compressors.Xz;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Utilities;
using static System.Net.Mime.MediaTypeNames;
using MusicFree.Models.AutenthicationModels;
using MusicFree.Models.InputModels;
using MusicFree.Models.ExtraModels;
using MusicFree.Models.GenreAndName;
using MusicFree.Models.DataReturnModel;
namespace MusicFree.Controllers
{

    [ApiController]
    public class MusicController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserContext _userContext;
        private readonly UserManager<User> _userManager;
        private readonly MusicService _ms;
        private readonly ContextMusicService _cms;
        public MusicController(UserManager<User> userManager, FreeMusicContext context)
        {
            _ms = new MusicService();
            _cms = new ContextMusicService(userManager,_context, _userContext);
            _userManager = userManager;
            _context = context;
        }


     



        [Route("music/musician_upload")]
        [HttpPost]
        public async Task<ActionResult> UploadMusician(UploadMusicians m)
        {

            Musician musician = new Musician(m.name, Guid.NewGuid().ToString());
            musician.liked_by = new List<MusicianUser>();
            if (musician.liked_by == null)
            {
                Console.WriteLine("haha");
            }
            _context.musicians.Add(musician);
            await _context.SaveChangesAsync();

            return Ok(new { src=musician.cover_src});
        }
        [Route("music/musician_img_upload")]
        [HttpPost]
        public async Task<ActionResult> UploadImgMusician()
        {
            var context = HttpContext;
            var image = new MemoryStream();
           GridFsPlayer ms = new GridFsPlayer();
            string boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 500);
            var multipartReader = new MultipartReader(boundary, context.Request.Body);
           
            while (await multipartReader.ReadNextSectionAsync() is { } section)
            {
                var section_name = MultipartRequestHelper.GetName(section.ContentDisposition);
                await section.Body.CopyToAsync(image);
                image.Position = 0;
                ms.UploadFile(section_name, image);
            }


            return Ok();
        }
 
        [Route("music/get_song/{filename}")]
        [HttpGet]
        public async Task<ActionResult> GetSong(string filename)
        {

            Console.WriteLine(24343443);
            Console.WriteLine("check3");
         
            var context = HttpContext;
           GridFsPlayer ms = new GridFsPlayer();

            await ms.StreamSong(context, filename);
         


       



            return new EmptyResult();
        }


   
       

        
           
       



           





        [Route("music/create_albumn/")]
        [HttpPost("music/create_albumn/")]
        public async Task<ActionResult> CreateAlbumn(Albumn_Create albumn_input)
        {
              Musician main_author = _context.musicians.Find(albumn_input.main_author);
            Console.WriteLine(main_author.Name);
            var extra_authors = new List<AlbumnAuthor>();

            Albumn album = new Albumn(albumn_input.name, main_author,albumn_input.albumn_type);
           

            IList<object> ReturnTagsCollection(List<string> id_list, bool is_genre, bool is_albumn, Song? song )
            {
                var result = new List<object>();
                foreach (var id in id_list)
                {
                    GenreTagCollection new_genre = null; 
                    if (is_genre)
                    { var genre_find =  _context.genres.Find(id);
                        if (genre_find == null) {
                            throw new Exception();
                        }
                        else
                        {


                            if (is_albumn)
                            {
                                new_genre = new GenretoAlbumn(genre_find, album);
                             album.genres.Add((GenretoAlbumn)new_genre);
                            genre_find.albumns.Add((GenretoAlbumn)new_genre);
                               
                            }
                            else {
                              new_genre = new GenretoSong(genre_find, song!);
                                song!.genres.Add((GenretoSong)new_genre);
                                genre_find.song.Add((GenretoSong)new_genre);
                               
                            }

                          
                        }
                    }
                    else
                    {
                        var tag_find = _context.tags.Find(id);
                        if (tag_find == null)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            if (is_albumn)
                            {
                                new_genre = new TagtoAlbumn(tag_find, album);
                                album.tags.Add((TagtoAlbumn)new_genre);
                                tag_find.albumns.Add((TagtoAlbumn)new_genre);
                            }
                            else
                            {
                                new_genre = new TagtoSong(tag_find, song);
                                song!.tags.Add((TagtoSong)new_genre);
                                tag_find.song.Add((TagtoSong)new_genre);
                            }
                        
                        }


                        }
                    result.Add(new_genre);
                }
                return result;

            }

          

            if (main_author.Albumns == null) {
                main_author.Albumns = new List<Albumn>();
            }
            Console.WriteLine(1);
            main_author.Albumns.Add(album);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message);

            }

            foreach (var item in albumn_input.extra_authors) 
            {
                var new_author = await _context.musicians.FindAsync(item);

                var new_object = new AlbumnAuthor(album, new_author);
                album.Extra_Authors.Add(new_object);
                new_author.collaboration_albumns.Add(new_object);
            }




            Console.WriteLine(2);
            foreach (var author_id in albumn_input.extra_authors)

            {
                Musician extra_author = _context.musicians.Find(author_id);
                AlbumnAuthor albumn_author = new AlbumnAuthor( album, extra_author);
                _context.albumn_authors.Add(albumn_author);
                await _context.SaveChangesAsync();
                extra_author.collaboration_albumns.Add(albumn_author);
                extra_authors.Add(albumn_author);

            }

            await _context.SaveChangesAsync();

            var filename_index = new List<Song_filename>();
            album.Extra_Authors = extra_authors;


            album.genres = ReturnTagsCollection(albumn_input.genres, true, true, null).Cast<GenretoAlbumn>().ToList();

            album.tags = ReturnTagsCollection(albumn_input.tags, false, true, null).Cast<TagtoAlbumn>().ToList();


            await _context.SaveChangesAsync();

            
            foreach (var genre in albumn_input.tags)
            {
                var albumn_tag = await _context.tags.FindAsync(genre);
                if (albumn_tag == null)
                {
                    return BadRequest();
                }
                else
                {
                    var albumn_to_tag = new TagtoAlbumn(albumn_tag, album);
                    album.tags.Add(albumn_to_tag);
                    albumn_tag.albumns.Add(albumn_to_tag);
                }


            }

            await _context.SaveChangesAsync();


            foreach (var song in albumn_input.songs)
            {

                Song new_song = new Song(song.name, main_author, album);
                // new_song.extra_authors.Add(GetSongAuthors(song.extra_authors, new_song)); изменить так что бы не было присваивания

                foreach (var item in song.extra_authors)
                {
                    var new_author = await _context.musicians.FindAsync(item);

                    var new_object = new SongAuthor(new_song, new_author);
                    new_song.extra_authors.Add(new_object);
                    new_author.collaboration_songs.Add(new_object);
                }

                var genres_id = new List<string>();
                var tags_id = new List<string>();  
                if (albumn_input.songs.Count==1) {
                    tags_id = albumn_input.tags;
                    genres_id = albumn_input.genres;
                    
                } else {
                    tags_id = song.tags;
                    genres_id = song.genres;
                }

                new_song.genres = ReturnTagsCollection(genres_id, true, false, new_song).Cast<GenretoSong>().ToList();
                new_song.tags = ReturnTagsCollection(tags_id, false, false, new_song).Cast<TagtoSong>().ToList();
                    new_song.Albumn = album;
                new_song.albumn_index = song.index;
                new_song.cover_src = album.cover_src;
                _context.songs.Add(new_song);
                if (main_author.Songs == null) {
                    main_author.Songs = new List<Song>();
                }
                main_author.Songs.Add(new_song);

                if (new_song.extra_authors.Any())
                {
                    main_author.collaboration_songs = main_author.collaboration_songs.Concat(new_song.extra_authors).ToList();
                    await _context.SaveChangesAsync();
                }

                    

                album.Songs.Add(new_song);
                new_song.cover_src = Guid.NewGuid().ToString();
                
            }
            Console.WriteLine(3);
            _context.albumns.Add(album);
            await _context.SaveChangesAsync();

            filename_index.Add(new Song_filename(album.cover_src, -1));
            return Ok(filename_index);
        }
        //перенести их в сервисы
       
        [Route("music/upload_albumn/")]
        [HttpPost]
        public async Task<ActionResult> UploadAlbumns()
        {
            var context = HttpContext;
            var song = new MemoryStream();
            GridFsPlayer ms = new GridFsPlayer();
            string boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 500);
            var multipartReader = new MultipartReader(boundary, context.Request.Body);
            while (await multipartReader.ReadNextSectionAsync() is { } section)
            {
                var name = MultipartRequestHelper.GetName(section.ContentDisposition);

                await section.Body.CopyToAsync(song);
                song.Position = 0;
                ms.UploadFile(name, song);
            }

            return Ok();
        }

        [Authorize(Roles = "Guest, User")]
        [Route("music/song_listened/{song_id}")]
        [Authorize]
        [HttpPost("music/song_listened/{song_id}")]
        public async Task<ActionResult> Song_listened(Guid song_id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var song = await  _context.songs.FindAsync(song_id);

            if (song.Albumn.albumn_views.Where(a => a.UserId == user.Id).Any())
            {

                song.Albumn.albumn_views.Where(a => a.UserId == user.Id).First().last_listened = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            else
            {
                var albumn_listened = new AlbumnViews(user.Id, song.Albumn.Id, song.Albumn, DateTime.Now);
                user.albumn_views.Add(albumn_listened.Id);
                song.Albumn.albumn_views.Add(albumn_listened);
                _context.albumn_views.Add(albumn_listened);
            }
            
            if (song.song_views.Where(a => a.UserId == user.Id).Any())
            {
                song.song_views.Where(a => a.UserId == user.Id).First().last_listened = DateTime.Now;
                song.song_views.Where(a => a.UserId == user.Id).First().listened++;
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (_context.songsViews.Where(a=> a.UserId==user.Id && a.SongId==song.Id).Any())
            {
                var to_change = _context.songsViews.Where(a => a.UserId == user.Id && a.SongId == song.Id).First();
                to_change.last_listened = DateTime.Now;
                to_change.listened++;
                await _context.SaveChangesAsync();
            }
            else
            {
                var song_listened = new SongViews(user.Id, song.Id, song);
            user.song_views.Add(song.Id);
            song.song_views.Add(song_listened);
            _context.songsViews.Add(song_listened);
            await _context.SaveChangesAsync();
            }
           


            return Ok();
        }

        [NonAction]
        public bool isUser(Song song, User user)
        {
          return _context.likes.Where(a => a.SongId == song.Id && a.UserId == user.Id).Any();
        }

        [Authorize(Roles ="Guest, User") ]
        [Route("last/songs/{page_size}")]
        public async Task<ActionResult> LastSongs(int page_size)
        {


               
            var ms = new MusicService();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var songviews = _context.songsViews.Where(a => a.UserId == user.Id).Take(10).ToList();
            var result = new List<SongReturn>();
            foreach (var songview in songviews) {
                
                result.Add(new SongReturn(songview.song, isUser(songview.song, user)));
            }
            return Ok(result);
        }

        [Authorize]
        [Route("last/search")]
        [HttpGet]
        public async Task<ActionResult> LastSearch()
        {
            


            var user = await _userManager.GetUserAsync(HttpContext.User);

           bool isSame( LastSearchParent a)
            {
                return a.UserId == user.Id;
            }

            DateTime LastSearchOrder(LastSearchParent a)
            {
                return a.last_searched;
            }

            
           ReturnParent SelectReturn(LastSearchParent a)
            {
                if(a.GetType() == typeof(SongLastSearch))
                {
                    var b = (SongLastSearch)a;
                    return new SongReturn(b.Song, isUser(b.Song, user));
                }
                else if (a.GetType() == typeof(AlbumnLastSearch))
                {
                    var b = (AlbumnLastSearch)a;
                    return new AlbumnReturn(b.Albumn);
                }
                else
                {
                    var b = (MusicianLastSearch)a;
                    return new AuthorReturn(b.Author);
                }
                
            }

            var musicians = _context.musicianlastSearches.Where(isSame).OrderBy(LastSearchOrder).Take(5)
             .Select(a=> new AuthorSearchReturn(a.last_searched,new AuthorReturn(a.Author))).ToList();
            var songs = _context.songlastSearches.Where(isSame).OrderBy(LastSearchOrder).Take(5)
                .Select(a=> new SongSearchReturn(a.last_searched, new SongReturn(a.Song,isUser(a.Song, user)))).ToList();
            var albumn = _context.albumnlastSearches.Where(isSame).OrderBy(LastSearchOrder).Take(5).Select(a => new AlbumnSearchReturn(a.last_searched,
                new AlbumnReturn(a.Albumn))).ToList();

            var result = new List<SearchReturn>();

            result = result.Concat(musicians).Concat(songs).Concat(albumn).OrderBy(a=> a.last_searched).ToList();

            return Ok(result.Select(a=> a.returnParent).ToList());
        }
        [Authorize]
        [Route("search/add_last/")]
        [HttpPost]
        public async Task<ActionResult> AddLastSearch( string type, Guid Id)// полностью переделать, поиск не через пользователя а через песню
        {
           var user = await _userManager.GetUserAsync(HttpContext.User);
          
            

            if (user.last_search.Count() > 20)
            {
              //  _context.Remove(user.last_search.OrderBy(a=>a.timestamp).Take(10));
                await _context.SaveChangesAsync();
            }
            
            if (type == "albumn")
            {
                var search = new SearchModel(user, true, _context.albumns.Find(Id));
                var user_search = new UserSearch(user.Id, search.Id, search.timestamp);
                _context.searches.Add(search);
                await _context.SaveChangesAsync();
               // user.last_search.Add(user_search);
                await _userManager.UpdateAsync(user);
            }
            else
            {

                var search = new SearchModel(user, true, _context.musicians.Find(Id));
                var user_search = new UserSearch(user.Id, search.Id, search.timestamp);
                _context.searches.Add(search);
                await _context.SaveChangesAsync();
              //  user.last_search.Add(user_search);
                await _userManager.UpdateAsync(user);
            }


            return Ok();
        }


        [Route("author/songs/{Id}")]
        [HttpGet]
        public async Task<ActionResult> AuthorMostPopularSongs( Guid Id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var songs = _context.musicians.Find(Id).Songs.OrderByDescending(a=>a.song_views.Count()).Take(20);
            var ms = new MusicService();
            var for_return = new List<SongReturn>();
            foreach (var song in songs) {

                
                for_return.Add(new SongReturn(song, isUser(song, user)));
            }
            return Ok(for_return);
        }

        
        [Route("author/subcriptions_count/{id}")]
        [HttpGet]
        public async Task<ActionResult> AuthorsSubcriptions(Guid Id)
        {
            var result = _context.musicians.Find(Id).liked_by.Count();

            return Ok(result);
        }

        [Route("music/delete_albumn/{id}")]
        [HttpGet]
        public async Task<ActionResult> DeleteAlbumn(Guid id)
        {
            var albumn = _context.albumns.Find(id);
            foreach(var song in albumn.Songs) { 
            _context.songs.Remove(song);
           await  _context.SaveChangesAsync();
            }
            _context.albumns.Remove(albumn);
            await _context.SaveChangesAsync();
            Console.WriteLine(55555);
            return Ok(albumn);  

        }

       

    }

}
