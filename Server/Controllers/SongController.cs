using System;
using System.Collections.Generic;
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
    public class SongController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public SongController(ApplicationDBContext context)
        {
            this._context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Get request");
            var users = await _context.Song.ToListAsync();
            return Ok(users);
        }
        
        [HttpGet("{title}")]
        public async Task<IActionResult> Get(string title)
        {
            Console.WriteLine("Get request");
            var users = await _context.Song.FirstOrDefaultAsync(s => s.title == title);
            return Ok(users);
        }

        [HttpPut("listen/{username}/{title}")]
        public async Task<IActionResult> Put(string username, string title)
        {
            try
            {
                var userListen = _context.UserSong.Single(us => us.title.Equals(title) && us.username.Equals(username));
                userListen.listen_count += 1;
                Console.WriteLine($"Updated {title} listen count to {userListen.listen_count} for {username}");
                _context.Update(userListen);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var userListen = new UserSong() {username = username, title = title, listen_count = 1};
                _context.Add(userListen);
                Console.WriteLine($"Added {title} listen count to {userListen.listen_count} for {username}");
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet("getfull/{num}")]
        public async Task<IActionResult> Get(int num)
        {
            var completeSongs = new List<SongComplete>();
            var songs = await _context.Song.ToListAsync();
            foreach (Song s in songs)
            {

                var artist_song = await _context.ArtistsSong.FirstAsync(song => song.title == s.title);
                var artist = artist_song.artist_name;
                var albums = await _context.SongOnAlbum.FirstAsync(song => song.title == s.title);
                var album = albums.album_name;
                var track = albums.track_number;
                completeSongs.Add(new SongComplete(){album = album, artist = artist, listenCount = track, song = s});
            }

            return Ok(completeSongs);
        }
    }
}