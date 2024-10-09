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
namespace MusicFree.Controllers
{
    
    [ApiController]
    public class MusicController : Controller
    {
        private readonly FreeMusicContext _context;
        public MusicController(FreeMusicContext context)
        {
            _context = context;
        }
        
      
        [HttpPost("music/musician_upload")]
        public async Task<ActionResult<Musician>> UploadMusician(UploadMusicians musiciann)
        {
            
            Musician musician = new Musician(musiciann.Name);
            _context.musicians.Add(musician);
            await _context.SaveChangesAsync();

            return musician;
        }
        
        [HttpPost("music/song_upload")]
        [DisableFormValueModelBinding]
        public async Task<ActionResult> UplaodSong(IOptions<JsonOptions> jsonOptions) {
            var name= "";
           Musician author = new Musician(name);
            var albumn = Guid.NewGuid();
            var context = HttpContext;
            var song = new MemoryStream();
            MusicService ms = new MusicService();
            var authors_id = new List<Guid>();
            string boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 500);
            var multipartReader = new MultipartReader(boundary, context.Request.Body);

            while (await multipartReader.ReadNextSectionAsync() is { } section)
            {
                var section_name = MultipartRequestHelper.GetName(section.ContentDisposition);
                Console.WriteLine(section_name);
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


                
                
                if(section_name == "albumn")
                {
                    StreamReader reader = new StreamReader(section.Body);
                    string for_albumn = reader.ReadToEnd(); ;
                    if(for_albumn == "")
                    {
                        albumn = Guid.Empty;
                    }
                    else
                    {
                        albumn = Guid.Parse(for_albumn);
                    }
                    
                }
                if (section_name =="song")
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
           
            
            _context.songs.Add(new_song);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("music/get_song/{filename}")]
        public async Task<ActionResult> GetSong(string filename)
        {

            
            var context = HttpContext;
            MusicService ms = new MusicService();
            await ms.StreamSong(context, filename);

           



            return Ok();
        }
        [HttpGet("music/find_aithor/{name}")]
        public async Task<ActionResult> FundAuthor(string name)
        {
            var authors_return = new List<AuthorReturn>();
            var authors = _context.musicians.Where(a=>a.Name==name).ToList();
            foreach (var author in authors)
            {
                authors_return.Add(new AuthorReturn(author.Id, author.Name));

            }
            return Ok(authors_return);
        }



        [HttpPost("music/create_albumn/")]
        public async Task<ActionResult> CreateAlbumn(Albumn_Create albumn_input)
        {
            Musician main_author = _context.musicians.Find(albumn_input.main_author);
            var extra_authors = new List<AlbumnAuthor>();
            
           Albumn album = new Albumn(albumn_input.name, main_author);
            main_author.Albumns.Add(album);
            await _context.SaveChangesAsync();

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
            foreach(var song in albumn_input.songs)
            {
               
                Song new_song = new Song(song.name, main_author);
                new_song.extra_authors=GetSongAuthors(song.extra_author,new_song);
                new_song.Albumn = album;
                new_song.albumn_index = song.index;
               _context.songs.Add(new_song);
                main_author.Songs.Add(new_song);
                main_author.collaboration_songs = main_author.collaboration_songs.Concat(new_song.extra_authors).ToList() ;
                   
                await _context.SaveChangesAsync();
                album.Songs.Add(new_song);
                new_song.song_filename = Guid.NewGuid().ToString();
                filename_index.Add(new Song_filename(new_song.song_filename, new_song.albumn_index.Value));
            }
            _context.albumns.Add(album);
            await _context.SaveChangesAsync();


            return Ok(filename_index);
        }
        //перенести их в сервисы
        [NonAction]
        public  List<SongAuthor> GetSongAuthors(List<Guid> authors_id, Song song)
        {
            var authors = new List<SongAuthor>();
            foreach(var author_id in authors_id)
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
        public  List<AlbumnAuthor> GetAlbumnAuthors(List<Guid> authors_id, Albumn albumn)
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
        [HttpPost("music/upload_albumn/")]
        public async Task<ActionResult> UploadAlbumns()
        {
            var context = HttpContext;
            var song = new MemoryStream();
            MusicService ms = new MusicService();
            string boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 500);
            var multipartReader = new MultipartReader(boundary, context.Request.Body);
            while (await multipartReader.ReadNextSectionAsync() is { } section)
            {
                var name = MultipartRequestHelper.GetName(section.ContentDisposition);
                await section.Body.CopyToAsync(song);
                ms.UploadFile(name, song);
            }
            return Ok();
            }


       

        [HttpGet("music/find_author/{name}")]
         public async Task<ActionResult<ICollection<AuthorReturn>>> AuthorDownload(string name)
        {    var results = _context.musicians.Where(a=>a.Name == name).ToList();
            
            var authors = new List<AuthorReturn>();
            foreach (var result in results) { 
                authors.Add(new AuthorReturn(result.Id, result.Name));

            }

            return authors;


        }
    } 

}
