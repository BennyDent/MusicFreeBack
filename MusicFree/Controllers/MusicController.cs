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
        
        [HttpPost("music/albumn_create")]
        public async Task<ActionResult<Albumn>> AlbumnCreate(AlbumnCreate ab)
        {var name = ab.Name;
         var author = _context.musicians.Find(ab.author_id);
            Albumn albumn = new Albumn(name, author);
            albumn.is_visible = false;
            author.Albumns.Add(albumn);
            _context.albumns.Add(albumn);
            await _context.SaveChangesAsync();
            return albumn;
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
                if (section_name == "author")
                {
                    StreamReader reader = new StreamReader(section.Body);
                    string text = reader.ReadToEnd();
                    var array = MultipartRequestHelper.AuthorStringSplit(text);
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
            
            Song new_song = new Song(name);
            if (albumn != Guid.Empty)
            {
               Albumn albumn_model = _context.albumns.Find(albumn);
                new_song.Albumn = albumn_model;  
            }
            new_song.song_filename = name;
           
            foreach (var author_id in authors_id)
            {
                Musician author_model = _context.musicians.Find(author_id);
                if (author_model == null)
                {

                }
                SongAuthor author = new SongAuthor(new_song.Id,author_model.Id, new_song, author_model);
                _context.song_authors.Add(author);
                author_model.Songs.Add(author);
                new_song.Authors.Add(author);
                await _context.SaveChangesAsync();
                
            }
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

        [HttpGet("music/find_albumn/{name}/{author_id}")]
        public async Task<ActionResult<ICollection<AuthorReturn>>>  GetAlbumn(string filename, Guid author_id) {
            var results = _context.albumns.Where(a=> a.Name==filename & a.Author.Id== author_id);
            var albumns = new List<AuthorReturn>();
            foreach (var result in results)
            {
                albumns.Add(new AuthorReturn(result.Id, result.Name));

            }
            return albumns;
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
