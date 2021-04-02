using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidwestMusicDB.Server.Data;

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
    }
}