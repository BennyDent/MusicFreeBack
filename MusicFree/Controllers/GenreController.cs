using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MusicFree.Models.AutenthicationModels;
using Microsoft.AspNetCore.Authorization;
using MusicFree.Models.DataReturnModel;
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
        
        


       

        
    }
}
