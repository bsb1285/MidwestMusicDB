using System;
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

        [HttpPut("/listen/{username}/{title}")]
        public async Task<IActionResult> Put(string username, string title)
        {
            try
            {
                var userListen = _context.UserSong.Single(us => us.title == title && us.username == username);
                userListen.listen_count += 1;
                _context.Update(userListen);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var userListen = new UserSong() {username = username, title = title, listen_count = 0};
                _context.Add(userListen);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}