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
        // [HttpGet("{username}/{id}")]
        // public async Task<IActionResult> Get(string username, int id)
        // {
        //     var userPlaylists =  _context.UsersPlaylist.Where(up => up.username == username).Select(up => up.id);
        //     var playlists = _context.Playlist.Where(p => userPlaylists.Contains(p.id));
        //     return Ok(playlists);
        // }

        [HttpGet("{id}/{songs}")]
        public async Task<IActionResult> Get(int id, bool songs)
        {
            if (songs)
            {
                var songsOnPlaylist = _context.SongOnPlaylist;
                var songTitles = songsOnPlaylist.Where(sop => sop.id == id).Select(sp => sp.title);
                var songs_list = _context.Song.Where(s => songTitles.Contains(s.title));
                return Ok(songs_list);
            }
            else
            {
                var p = _context.Playlist.Single(ps => ps.id == id);
                return Ok(p);
            }
           
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> Post(Playlist playlist, string username)
        {
            var next_id = _context.Playlist.Last().id + 1;
            playlist.id = next_id;
            UsersPlaylist up = new UsersPlaylist {id = playlist.id, username = username};
            _context.Add(up);
            _context.Add(playlist);
            await _context.SaveChangesAsync();
            return Ok(playlist.title);
        }

        [HttpPut("{id}/{title}")]
        public async Task<IActionResult> Put( int id, string title)
        {
            var song = _context.Song.First(s => s.title == title);
            var playlist = _context.Playlist.Single(p => p.id == id);
            var sop = new SongOnPlaylist() {id = id, title = title};
            playlist.song_count++;
            playlist.total_duration += song.length;
            _context.Update(playlist);
            _context.Add(sop);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("update/{newname}/{id}")]
        public async Task<IActionResult> Put(string newname, int id)
        {
            var playlist = _context.Playlist.Single(p => p.id == id);
            playlist.title = newname;
            _context.Update(playlist);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("remove/{id}/{title}")]
        public async Task<IActionResult> Delete(int id, string title)
        {
            var playlist = _context.Playlist.Single(ps => ps.id == id);
            var song = _context.Song.Single(s => s.title == title);
            playlist.song_count--;
            playlist.total_duration -= song.length;
            var up = _context.SongOnPlaylist
                .Single(sop => sop.id == id && sop.title == title);
            _context.Remove(up);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}