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
namespace MusicFree.Controllers
{

    [ApiController]
    public class MusicController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserManager<User> _userManager;
        public MusicController(UserManager<User> userManager, FreeMusicContext context)
        {
            _userManager = userManager;
            _context = context;
        }


     



        [Route("music/musician_upload")]
        [HttpPost]
        public async Task<ActionResult> UploadMusician(UploadMusicians m)
        {

            Musician musician = new Musician(m.name);
            musician.liked_by = new List<MusicianUser>();
            if (musician.liked_by == null)
            {
                Console.WriteLine("haha");
            }
            _context.musicians.Add(musician);
            await _context.SaveChangesAsync();

            return Ok(new { src=musician.img_filename});
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
        [Route("music/song_upload")]
        [HttpPost("music/song_upload")]
        [DisableFormValueModelBinding]
        public async Task<ActionResult> UplaodSong(IOptions<JsonOptions> jsonOptions) {
            var name = "";
            Musician author = new Musician(name);
            var albumn = Guid.NewGuid();
            var context = HttpContext;
            var song = new MemoryStream();
            GridFsPlayer ms = new GridFsPlayer();
            var authors_id = new List<Guid>();
            var song_src = "";
            string boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 500);
            var image = new MemoryStream();
            var multipartReader = new MultipartReader(boundary, context.Request.Body);

            while (await multipartReader.ReadNextSectionAsync() is { } section)
            {
                var section_name = MultipartRequestHelper.GetName(section.ContentDisposition);
                Console.WriteLine(section_name);
                if (section_name == "cover")
                {
                    song_src = Guid.NewGuid().ToString();
                    await section.Body.CopyToAsync(image);
                    image.Position = 0;
                    ms.UploadFile(song_src, image);

                }

                if (section_name == "name")
                {
                    StreamReader reader = new StreamReader(section.Body);
                    string text = reader.ReadToEnd();
                    name = text;

                    Console.WriteLine(name);
                }
                if (section_name == "main_author")
                {
                    StreamReader reader = new StreamReader(section.Body);
                    string text = reader.ReadToEnd();
                    var author_id = Guid.Parse(text.Split('"')[1].Split('"')[1]);
                    author = _context.musicians.Find(author_id);
                }

                if (section_name == "extra_authors")
                { StreamReader reader = new StreamReader(section.Body);
                    string text = reader.ReadToEnd();
                    var array = MultipartRequestHelper.AuthorStringSplit(text);
                    //добавить
                    var extra_authors = new List<Musician>();
                    foreach (var item in array)
                    {

                        var author_id = Guid.Parse(item.Split('"')[1].Split('"')[1]);
                        authors_id.Add(author_id); }
                }




                if (section_name == "albumn")
                {
                    StreamReader reader = new StreamReader(section.Body);
                    string for_albumn = reader.ReadToEnd(); ;
                    if (for_albumn == "")
                    {
                        albumn = Guid.Empty;
                    }
                    else
                    {
                        albumn = Guid.Parse(for_albumn);
                    }

                }
                if (section_name == "song")
                {
                    await section.Body.CopyToAsync(song);
                }
            }
            var for_filename = Guid.NewGuid();
            var file_name = for_filename.ToString();

            ms.UploadFile(file_name, song);


            Song new_song = new Song(name, author);
            if (albumn != Guid.Empty)
            {
                Albumn albumn_model = _context.albumns.Find(albumn);
                new_song.Albumn = albumn_model;
            }
            new_song.song_filename = name;
            new_song.cover_filename = song_src;

            _context.songs.Add(new_song);
            await _context.SaveChangesAsync();
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

            Albumn album = new Albumn(albumn_input.name, main_author);

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
            Console.WriteLine(2);
            foreach (var author_id in albumn_input.extra_authors)

            {
                Musician extra_author = _context.musicians.Find(author_id);
                AlbumnAuthor albumn_author = new AlbumnAuthor(album.Id, author_id, album, extra_author);
                _context.albumn_authors.Add(albumn_author);
                await _context.SaveChangesAsync();
                extra_author.collaboration_albumns.Add(albumn_author);
                extra_authors.Add(albumn_author);

            }

            var filename_index = new List<Song_filename>();
            album.Extra_Authors = extra_authors;
            foreach (var song in albumn_input.songs)
            {

                Song new_song = new Song(song.name, main_author);
               // new_song.extra_authors.Add(GetSongAuthors(song.extra_authors, new_song)); изменить так что бы не было присваивания
                new_song.Albumn = album;
                new_song.albumn_index = song.index;
                new_song.cover_filename = album.cover_filename;
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
                new_song.song_filename = Guid.NewGuid().ToString();
                filename_index.Add(new Song_filename(new_song.song_filename, new_song.albumn_index.Value));
            }
            Console.WriteLine(3);
            _context.albumns.Add(album);
            await _context.SaveChangesAsync();

            filename_index.Add(new Song_filename(album.cover_filename, -1));
            return Ok(filename_index);
        }
        //перенести их в сервисы
        [NonAction]
        public List<SongAuthor> GetSongAuthors(List<Guid> authors_id, Song song)
        {
            var authors = new List<SongAuthor>();
            foreach (var author_id in authors_id)
            {
                Musician extra_author = _context.musicians.Find(author_id);
                SongAuthor song_author = new SongAuthor(song.Id, author_id, song, extra_author);
                _context.song_authors.Add(song_author);
                _context.SaveChangesAsync();
                authors.Add(song_author);
            }
            return authors;
        }
        [NonAction]
        public List<AlbumnAuthor> GetAlbumnAuthors(List<Guid> authors_id, Albumn albumn)
        {
            var authors = new List<AlbumnAuthor>();
            foreach (var author_id in authors_id)
            {
                Musician extra_author = _context.musicians.Find(author_id);
                AlbumnAuthor song_author = new AlbumnAuthor(albumn.Id, author_id, albumn, extra_author);
                _context.albumn_authors.Add(song_author);
                _context.SaveChangesAsync();
                authors.Add(song_author);
            }
            return authors;
        }
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


        [Route("music/song_listened/{song_filename}")]
        [Authorize]
        [HttpPost("music/song_listened/{song_filename}")]
        public async Task<ActionResult> Song_listened(string song_filename)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var song = _context.songs.Where(a => a.song_filename == song_filename).First();
           
            if (song.Albumn != null)
            {
                if(song.Albumn.albumn_views.Where(a=> a.UserId == user.Id).FirstOrDefault()!= null)
                {
                    song.Albumn.albumn_views.Where(a => a.UserId == user.Id).First().last_listened = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var albumn_listened = new AlbumnViews(user.Id, song.Albumn.Id,  song.Albumn, DateTime.Now);
                    user.albumn_views.Add(albumn_listened.Id);
                    song.Albumn.albumn_views.Add(albumn_listened);
                    _context.albumn_views.Add(albumn_listened);
                }
            }
            if (song.song_views.Where(a => a.UserId == user.Id).FirstOrDefault() != null)
            {
                song.song_views.Where(a => a.UserId == user.Id).First().last_listened = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok();
            }
            var song_listened = new SongViews(user.Id, song.Id, user, song, DateTime.Now);
            user.song_views.Add(song_listened.Id);
            song.song_views.Add(song_listened);
            _context.songsViews.Add(song_listened);
            await _context.SaveChangesAsync();


            return Ok();
        }


        [Route("last/songs/{page_size}")]
        [Authorize]
        public async Task<ActionResult> LastSongs(int page_size)
        {


               
            var ms = new MusicService();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var songviews = _context.songsViews.Where(a => a.UserId == user.Id).OrderByDescending(a => a.song_listened).Take(10).ToList();
            var result = new List<SongReturn>();
            foreach (var songview in songviews) {
                var song = songview.song;
                result.Add(ms.SongToSongReturn(song, user));
            }
            return Ok(result);
        }

        [Route("last/search")]
        [HttpGet]
        public async Task<ActionResult> LastSearch()
        { var ms = new MusicService();
            var list = new List<Object>();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user==null) {
                return Unauthorized();
            }
            var result = user.last_search.Take(5);
            foreach (var part in result)
            {
                var search = await _context.searches.FindAsync(part);

                if (search.is_albumn)
                {
                    list.Add(new { type = "albumn", result = ms.AlbumnReturn(search.Albumn) });
                }
                else
                {
                    list.Add(new { type = "author", result = new AuthorReturn(search.Author.Id, search.Author.Name) });
                }



            }

            return Ok(list);
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
                for_return.Add(ms.SongToSongReturn(song, user));
            }
            return Ok(for_return);
        }
        [Route("author/albumns/{Id}")]
        [HttpGet]
        public async Task<ActionResult> AlbumnMostPopularSongs(Guid Id)
        {
            var ms = new MusicService(); ;

            var albumns = _context.musicians.Find(Id).Albumns.OrderByDescending(a => ms.AlbumnViews(a)).Take(5);
            var for_return = new List<AlbumnRethurn>();
            foreach (var song in albumns)
            {
                for_return.Add(ms.AlbumnReturn(song));
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
