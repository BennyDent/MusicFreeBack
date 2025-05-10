using Microsoft.AspNetCore.Mvc;
using MusicFree.Models;
using Microsoft.AspNetCore.Identity;


namespace MusicFree.Controllers
{
    [ApiController]
    public class RecomendationController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserManager<User> _userManager;
        public RecomendationController(UserManager<User> userManager, FreeMusicContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [NonAction]
        public  Boolean GenreTagCompare(Musician Author, Musician Main_Author, Boolean is_Similar) {

            var is_Similar_genre = false;

            if (is_Similar) { 
            foreach (var genre in Main_Author.genres){
                    if (Author.genres.Intersect(Main_Author.genres).Count()>0) {
                    is_Similar_genre=true;
                        break;
                    }
                }
            }
            
            if (Main_Author.genres.Intersect(Author.genres).Count() > 0 | is_Similar_genre) {

                if (Author.tags.Intersect(Main_Author.tags).Count()>2)
                {
                    return true;
                }

            }
            return false;
        
        }

        [Route("")]
        [HttpPost("/similar/authors/{id}")]
        public async Task<ActionResult> SimilarAuthors(string id, Boolean is_popular)
        {
            var author = _context.musicians.Find(id);
            List<Musician> authors = null ;
            List<Musician> similar_authors = null ; 
            if (is_popular) {
            
             authors = _context.musicians.Where(a=>GenreTagCompare(a, author, false)).OrderByDescending(a=>a.liked_by.Count()).Take(5).ToList();
            similar_authors = _context.musicians.Where(a => GenreTagCompare(a, author, true)).OrderByDescending(a => a.liked_by.Count()).Take(5).ToList();
            } else {
                authors = _context.musicians.Where(a => GenreTagCompare(a, author, false)).OrderBy(a => a.liked_by.Count()).Take(5).ToList();
                similar_authors = _context.musicians.Where(a => GenreTagCompare(a, author, true)).OrderBy(a => a.liked_by.Count()).Take(5).ToList();
            }
            var return_result = new List<Object>();
            foreach (var Author in authors) {
                return_result.Add(new {name=Author.Name, id=Author.Id,src=Author.img_filename, });
            }
            foreach (var Author in similar_authors)
            {
                return_result.Add(new { name = Author.Name, id = Author.Id, src = Author.img_filename, });
            }
            return Ok(return_result);
        } 

        public IActionResult Index()
        {
            return View();
        }
    }
}
