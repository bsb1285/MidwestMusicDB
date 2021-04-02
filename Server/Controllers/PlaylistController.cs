using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidwestMusicDB.Server.Data;
using MidwestMusicDB.Shared.Models;

namespace MidwestMusicDB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public PlaylistController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var usersPlaylists = await _context.UsersPlaylist.ToListAsync();
            var ids = usersPlaylists.Where(playlist => playlist.username == username).Select(p => p.id);

            var playlists = await _context.Playlist.ToListAsync();
            var playlistsForUsers = playlists.Where(p => ids.Contains(p.id));
            return Ok(playlistsForUsers);
        }
        [HttpGet("{username}/{id}")]
        public async Task<IActionResult> Get(string username, int id)
        {
            var userPlaylists =  _context.UsersPlaylist.Where(up => up.username == username).Select(up => up.id);
            var playlists = _context.Playlist.Where(p => userPlaylists.Contains(p.id));
            return BadRequest();
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> Post(Playlist playlist, string username)
        {
            UsersPlaylist up = new UsersPlaylist {id = playlist.id, username = username};
            _context.Add(up);
            _context.Add(playlist);
            await _context.SaveChangesAsync();
            return Ok(playlist.title);
        }

        [HttpPost("{username}/{id}/{title}")]
        public async Task<IActionResult> Post(string username, int id, string title)
        {
            return BadRequest();
        }
        [HttpPut("/{username}/{id}")]
        public async Task<IActionResult> Put(string username, int id)
        {
            return BadRequest();
        }
    }
}