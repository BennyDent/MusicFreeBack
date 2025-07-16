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

            if (Input.is_tag)
            {
                var tag = new Tags(Input.name);
                foreach(var similar_id in Input.similar)
                { var similar_tag = await _context.tags.FindAsync(similar_id);
                    if (similar_tag == null) {
                        return BadRequest();
                    }
                    var new_tags = new TagTag(similar_tag, tag);
                    tag.similar.Add(new_tags);
                    _context.similar_tag.Add(new_tags);
                }
                _context.tags.Add(tag);
                await _context.SaveChangesAsync();
            }
            else
            {
                var genre = new Genre(Input.name);
                foreach (var similar_id in Input.similar)
                {
                    var similar_tag = await _context.genres.FindAsync(similar_id);
                    if (similar_tag == null)
                    {
                        return BadRequest();
                    }
                    var new_tags = new GenreGenre(similar_tag, genre);
                    genre.similar.Add(new_tags);
                    _context.similar_genre.Add(new_tags);
                }
                _context.genres.Add(genre);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        


       

        
    }
}
