using Microsoft.AspNetCore.Mvc;
using MusicFree.Models;

using Microsoft.AspNetCore.Identity;
using MusicFree.Models.DataReturnModel;

namespace MusicFree.Controllers
{
    public class ListenedController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserContext _userContext;
        private readonly UserManager<User> _userManager;
        private SongReturn _songRethurn;
        private bool is_Changed;
        public ListenedController(UserManager<User> userManager, FreeMusicContext context)
        {
            _userManager = userManager;
            _context = context;
        }



        



    }
}
