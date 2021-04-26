using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidwestMusicDB.Server.Data;
using MidwestMusicDB.Shared.Models;

namespace MidwestMusicDB.Server.Controllers
{
    public class FollowerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public FollowerController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("{followee}/{follower}")]
        public async Task<IActionResult> Post(string followee, string follower)
        {
            _context.UsersFollower.Add(new UserFollower() {follower_username = follower, username = followee});
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{followee}/{follower}")]
        public async Task<IActionResult> Delete(string followee, string follower)
        {
            var uf =
                _context.UsersFollower.Single(userF => userF.follower_username == follower && userF.username == followee);
            _context.Remove(uf);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("{followee}/{follower}")]
        public async Task<IActionResult> Get(string followee, string follower)
        {
            var uf = _context.UsersFollower.ContainsAsync(new UserFollower()
                {follower_username = follower, username = followee});
            return Ok(uf);
        }
    }
}