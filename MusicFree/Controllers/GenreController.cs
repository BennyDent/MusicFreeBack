using Microsoft.AspNetCore.Mvc;
using MusicFree.Models;
using Microsoft.AspNetCore.Identity;
using MusicFree.Models.AutenthicationModels;
using Microsoft.AspNetCore.Authorization;
using MusicFree.Models.DataReturnModel;
using Microsoft.AspNet.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MusicFree.Models.InputModels;
using MailKit.Search;
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
            if (Input.is_tag)
            {
                new Tags(Input.name);
            }
            else {
            new Genres(Input.name);
            }
            return Ok();
        }

        


        [Route("")]
        [HttpGet("/genre_tag/find/{is_tag}/{name}/{page_index}")]
        public async Task<IActionResult> GetGenreorTag(string name, string is_tag, int page_index)
        {
            var page_size = 5;
            var hasMore = true;
            var result=  new List<GenreAndName>();
            var results_array = new List<NameIdReturnData>();
            if (is_tag=="tags")
            {
                result = _context.tags.Where(a => a.Name==name).OrderBy(a=>a.songs.Count()).Take(page_size*page_index).Cast<GenreAndName>().ToList();
            }
            else
            {
                result = _context.tags.Where(a => a.Name == name).OrderBy(a => a.songs.Count()).Take(page_size * page_index).Cast<GenreAndName>().ToList();
            }

            if (result.Count<=5) { 
            hasMore = false;    
            }

            foreach (var item in result) {
            results_array.Add(new NameIdReturnData(item.Name, item.Id));    
            }
                

            return Ok(new {hasMore=hasMore,});
        }
    }
}
