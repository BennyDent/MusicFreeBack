using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MusicFree.Models.AutenthicationModels;
using Microsoft.AspNetCore.Authorization;
using MusicFree.Models.DataReturnModel;
using Microsoft.AspNet.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MusicFree.Models.InputModels;
using MailKit.Search;
using MusicFree.Models.GenreAndName;
namespace MusicFree.Controllers
{
    public class GenreController : Controller
    {

        private readonly FreeMusicContext _context;

        public GenreController(FreeMusicContext context)
        {
            _context = context;
        }
        [Route("")]
        [HttpPost("/genre_tag/create")]
        public async Task<IActionResult> CreateGenreorTag(GenreTagInput Input)
        {
            var new_genre_tag = new Models.GenreAndName.GenreAndName(Input.name, false);
            var similars = new List<Models.GenreAndName.GenreAndName>();
            foreach(var tag in Input.similar)
            {
                Models.GenreAndName.GenreAndName new_genre;
                if (Input.is_tag)
                {
                    new_genre = _context.tags.Find(tag);
                }else { new_genre = _context.genres.Find(tag); }
        if(new_genre== null)
                {
                    return BadRequest();
                }
        similars.Add(new_genre);       
            }
            return Ok();
        }

        


        [Route("")]
        [HttpGet("/genre_tag/find/{is_tag}/{name}/{page_index}")]
        public async Task<IActionResult> GetGenreorTag(string name, string is_tag, int page_index)
        {
            var page_size = 5;
            var hasMore = true;
            var result=  new List<Models.GenreAndName.GenreAndName>();
            var results_array = new List<NameIdReturnData>();
            if (is_tag=="tags")
            {
                result = _context.tags.Where(a => a.Name==name).OrderBy(a=>a.songs.Count()).Take(page_size*page_index).Cast<Tag>().ToList();
            }
            else
            {
                result = _context.tags.Where(a => a.Name == name).OrderBy(a => a.songs.Count()).Take(page_size * page_index).Cast<Tag>().ToList();
            }

            if (result.Count<=5) { 
            hasMore = false;    
            }

            foreach (var item in result) {
            results_array.Add(new NameIdReturnData(item.Name));    
            }
                

            return Ok(new {hasMore=hasMore,});
        }

        
    }
}
