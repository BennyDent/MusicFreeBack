
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicFree.Models;
using MusicFree.Models;
using MusicFree.Models.AutenthicationModels;
using MusicFree.Models.DataReturnModel;
using MusicFree.Services;
using System.Diagnostics.Eventing.Reader;
namespace MusicFree.Controllers
{
    [ApiController]
    public class LikesAndPlaylistsController : Controller
    {
        private readonly FreeMusicContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ContextMusicService _cms;
        public LikesAndPlaylistsController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
           FreeMusicContext context)
        {
            _context = context;
            _cms = new ContextMusicService(userManager, _context);
        }

        [Authorize]
        [HttpGet("playlist/playlist_search/{find}/{pag_index}")]
        public async Task<ActionResult> Last_Playlist(string find, int pag_index)
        {
            var page_size = 5;
            var ishasMore = true;
            var user = await _cms.ReturnUserModel(HttpContext.User);

            var playlists = _context.playlist.Where(a => a.author.Id == user.Id).OrderBy(a => a.timestamp).Take(pag_index * page_size).ToList();

            var playlist_return = new List<NameIdReturnData>();

            if (playlists.Count <= 5)
            {
                ishasMore = false;
            }

            foreach (var playlist in playlists)
            {
                playlist_return.Add(new NameIdReturnData(playlist.Name));
            }

            return Ok(new { hasMore = ishasMore, playlists = playlist_return });
        }

        [Authorize]
        [HttpPost("playlists/add_to_playlist")]
        public async Task<ActionResult> Add_To_Playlist(Add_To_Playlist input)
        {
            var user = await _cms.ReturnUserModel(HttpContext.User);
            var song = await _context.songs.FindAsync(input.SongId);
            var playlist = await _context.playlist.FindAsync(input.PlaylistId);
            if (user.Id != playlist.author.Id)
            {
                return Unauthorized();
            }
            if (_context.playlistSong.Where(a => a.Song == song && a.Playlist == playlist).Any())
            {
                return BadRequest();
            }
            var new_playlistsong = new PlaylistSong(song.Id, playlist.Id, song, playlist);

            return Ok();
        }

        [Authorize]
        [HttpPost("playlist/create_playlist")]
        public async Task<ActionResult> Create_Playlist(CreatePlaylist input)
        {
            var user = await _cms.ReturnUserModel(HttpContext.User);
            var new_playlist = new Playlist(input.name, user);
            _context.playlist.Add(new_playlist);
            await _context.SaveChangesAsync();
            if (input.songs.Count() != 0)
            {
                foreach (var song_id in input.songs)
                {
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
            var user = await _cms.ReturnUserModel(HttpContext.User);
            Console.WriteLine(user.Id);


            var song = await _context.songs.FindAsync(input.song_Id);


            var like = _context.likes.Where(a => a.SongId == input.song_Id && a.UserId == user.Id).First();

            if (like != null)
            {
                //_context.likes.Remove(_context.likes.Where(a => a.UserId == user.Id).First());


                _context.likes.Remove(like);



                await _context.SaveChangesAsync();


                return Ok(new { is_liked = false });



            }
            else
            {
                var new_like = new UserSong(user, song);
                song.liked_by.Add(new_like);
                _context.likes.Add(new_like);
                var res = await _context.SaveChangesAsync();

                song.liked_by.Add(new_like);
                user.song_likes.Add(new_like);

                Console.WriteLine(song.liked_by.Where(a => a.UserId == user.Id).Any());

                Console.WriteLine(14);
                return Ok(new { is_liked = true });
            }

            // user.song_likes.Remove(user.song_likes.Where(a=> a==input.song_Id).First());



        }







    }


}