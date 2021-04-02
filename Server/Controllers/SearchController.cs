using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MidwestMusicDB.Server.Data;

namespace MidwestMusicDB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public SearchController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpGet("{searchString}")]
        public async Task<IActionResult> Get(string searchString)
        {
            
            return Ok(_context.Song.First(s=> s.song_release_date == 2000));
        }
    }
}