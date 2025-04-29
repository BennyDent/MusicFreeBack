using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using MusicFree.Models;
using Microsoft.AspNetCore.Identity;
using MusicFree.Models.AutenthicationModels;
using MusicFree.Models;
using Microsoft.AspNetCore.Authorization;
using MusicFree.Models.DataReturnModel;
namespace MusicFree.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        private readonly FreeMusicContext _context;
 
        public HomeController( FreeMusicContext context)
        {
            _context = context;
        }
      

    }
}
