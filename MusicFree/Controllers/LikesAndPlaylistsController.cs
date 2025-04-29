
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
    public class LikesAndPlaylistsController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
 
        public LikesAndPlaylistsController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
           FreeMusicContext context)
        {
            _context = context;
            _userManager = userManager;
          
        }

        [Authorize]
        [HttpGet("playlist/playlist_search/{find}/{pag_index}")]
        public async Task<ActionResult> Last_Playlist( string find, int pag_index )
        {
            var page_size = 5;
            var ishasMore = true;
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var playlists = _context.playlist.Where(a=> a.AuthorId == user.Id).OrderBy(a => a.timestamp).Take(pag_index*page_size).ToList();

            var playlist_return = new List<NameIdReturnData>();

            if (playlists.Count<=5) {
            ishasMore = false;
            }

            foreach(var playlist in playlists) {
                playlist_return.Add(new NameIdReturnData(playlist.Name,playlist.Id));
            }

            return Ok(new { hasMore = ishasMore, playlists = playlist_return });
        }

        [Authorize]
        [HttpPost("playlists/add_to_playlist")]
        public async Task<ActionResult> Add_To_Playlist(Add_To_Playlist input)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var song = await _context.songs.FindAsync(input.SongId);
            var playlist = await _context.playlist.FindAsync(input.PlaylistId);
            if (user.Id !=playlist.AuthorId )
            {
                return Unauthorized();
            }
            if (_context.playlistSong.Where(a=> a.Song==song && a.Playlist==playlist).Any()) {
                return BadRequest();
            }
            var new_playlistsong = new PlaylistSong(song.Id,playlist.Id, song, playlist);

            return Ok();
        }

        [Authorize]
        [HttpPost("playlist/create_playlist")]
        public async Task<ActionResult> Create_Playlist(CreatePlaylist input)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var new_playlist = new Playlist(input.name, user.Id);
            _context.playlist.Add(new_playlist);
            await _context.SaveChangesAsync();
            user.playlists.Add(new_playlist.Id);
            await _userManager.UpdateAsync(user);
            if (input.songs.Count() != 0 ) {
            foreach (var song_id in input.songs) {
                    var song = await _context.songs.FindAsync(song_id);
                    var new_playlistsong = new PlaylistSong(song_id, new_playlist.Id, song, new_playlist);
                    _context.playlistSong.Add(new_playlistsong);
                    song.playllists.Add(new_playlistsong);
                    new_playlist.songs.Add(new_playlistsong);
                    await _context.SaveChangesAsync();
                }
            
            }

            return Ok();
        }
        
        
        
        [Authorize]
        [HttpPost("likes/add_like")]
        public async Task<IActionResult> Add_like(LikeInput input)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Console.WriteLine(user.Id);
         
           
             var song = await _context.songs.FindAsync(input.song_Id);
           
            if(user.song_likes == null)
            {
                Console.WriteLine(19);
 user.song_likes = new List<Guid>();
                var result = await _userManager.UpdateAsync(user);
          
            }
             Console.WriteLine(user.song_likes.Count());
            Console.WriteLine(_context.likes.Where(a => a.Song == song).Any());
            if ( _context.likes.Where(a=> a.Song == song).Any())
            {
                //_context.likes.Remove(_context.likes.Where(a => a.UserId == user.Id).First());
                
                foreach (var like in _context.likes.Where(a => a.UserId == user.Id).ToList()){
                    _context.likes.Remove(like);
                    
                }
                foreach (var like in user.song_likes.Where(a => a == song.Id).ToList())
                {
                    user.song_likes.Remove(like);

                }

                // user.song_likes.Remove(user.song_likes.Where(a=> a==input.song_Id).First());

                await _context.SaveChangesAsync();
                 await _userManager.UpdateAsync(user);
                Console.WriteLine((song.liked_by.Where(a => a.UserId == user.Id).Any()));
                Console.WriteLine(13);
                return Ok(new {is_liked=false});

            }
           
            var new_like = new UserSong(input.song_Id, user.Id,song);
            song.liked_by.Add(new_like);
            _context.likes.Add(new_like);
            var res = await _context.SaveChangesAsync();
           
            song.liked_by.Add(new_like);
            var resultvbv = await _context.SaveChangesAsync();
            Console.WriteLine(res);
            Console.WriteLine(resultvbv);
          Console.WriteLine(16);
         
            user.song_likes.Add(input.song_Id);
            var result1 =  await _userManager.UpdateAsync(user);
            Console.WriteLine(result1.Succeeded);
            Console.WriteLine(song.liked_by.Where(a => a.UserId == user.Id).Any());
           
            Console.WriteLine(14);
            return Ok(new {is_liked=true});
        }
        
    }
}
