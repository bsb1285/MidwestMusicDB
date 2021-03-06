using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MidwestMusicDB.Shared.Models;
using MidwestMusicDB.Server.Data;
using System.Threading.Tasks;
using System.Linq;

namespace MidwestMusicDB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public UserController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{username}/{data}")]
        public async Task<IActionResult> Get(string username, int data)
        {
            switch (data)
            {
                case 0:
                {
                    var users = await _context.Users.FirstOrDefaultAsync(a => a.username == username);
                    return Ok(users);
                }
                case 1:
                {
                    var userFollower = await _context.UsersFollower.ToListAsync();
                    
                    var followCount =  userFollower.Count(uf => uf.follower_username.Equals(username) );
                    var followerCount =  userFollower.Count(uf => uf.username.Equals(username));
                    return Ok(new int[] {followCount, followerCount});
                }
                default:
                {
                    var following = _context.UsersFollower
                        .Where(uf => uf.follower_username == username)
                        .Select(uf => uf.username);
                    return Ok(following);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user.username);
        }

        [HttpPut]
        public async Task<IActionResult> Put(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = new User {username = username};
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Post(LoginRequest loginRequest)
        {
            var u = await _context.Users.FirstOrDefaultAsync(a =>
                a.username == loginRequest.username && a.password == loginRequest.password);
            if (u != null)
            {
                Console.WriteLine("Good user input");
                u.last_access_date = DateTime.Today;
                await Put(u);
                return Ok();
            }

            Console.WriteLine("Bad user input");
            return BadRequest("Username or password is incorrect");
        }

        
    }
}