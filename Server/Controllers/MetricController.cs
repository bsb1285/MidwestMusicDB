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
    public class MetricController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public MetricController(ApplicationDBContext context)
        {
            _context = context;
        }
        
        [HttpGet("{username}/{type}")]
        public async Task<IActionResult> Get(string username ,string type)
        {
            Console.WriteLine("In Get");
            switch (type)
            {
                case "artists":
                    var artists = GetTopArtists(username, 10);
                    return Ok(artists);
                case "friends":
                    var friendsSongs = await GetTopSongsFriends(username, 50);
                    return Ok(friendsSongs);
                case "foryou":
                    var foryou = await BuildForYou(username);
                    return Ok(foryou);
                default:
                    return Ok(new string[] {"this","should","work"});
            }
        }

        [HttpGet("global/{type}")]
        public async Task<IActionResult> Get(string type)
        {
            switch (type)
            {
                case "songs" :
                    var songs = await GetTopSongsGlobal(50);
                    return Ok(songs);
                case "genre":
                    var topGenres = await GetTopGenres(null, 5);
                    return Ok(topGenres);
                default:
                    return Ok(new[] {"not sure", "why"});
            }
        }

        private List<string> GetTopArtists(string username, int amount)
        {
            Console.WriteLine("Start Artists");
            var topSongs = GetTopSongTitles(username, amount);
            var topArtists = new List<String>();
            foreach (var title in topSongs)
            {
                topArtists.Add(_context.ArtistsSong.
                    First(aS => aS.title == title)
                    .artist_name);
            }

            Console.WriteLine("End Artists");
            return topArtists;
        }

        private List<string> GetTopSongTitles(string username, int amount)
        {
            
            var usersongs = _context.UserSong.Where(us => us.username == username)
                .OrderByDescending(u => u.listen_count).Take(amount);
            return usersongs.Select(s => s.title).ToList();
        }

        private async Task<IEnumerable<string>> GetTopSongsGlobal(int amount)
        {
            var usersongs = await _context.UserSong.ToListAsync();
            var songDict = CreateSongDict(usersongs);

            var top = songDict.OrderByDescending(kp => kp.Value)
                .Select(kp => kp.Key).Take(amount);

            return top;
        }

        private static Dictionary<string, int> CreateSongDict(List<UserSong> usersongs)
        {
            var songDict = new Dictionary<string, int>();
            foreach (var us in usersongs)
            {
                var title = us.title;
                if (!songDict.ContainsKey(title))
                {
                    songDict.Add(title, us.listen_count);
                }
                else
                {
                    songDict[title] += us.listen_count;
                }
            }

            return songDict;
        }

        private async Task<IEnumerable<string>> GetTopGenres(string username,int amount)
        {
            var userSong = await _context.UserSong.ToListAsync();
            if (username != null)
            {
                userSong = await _context.UserSong.Where(us => us.username == username).ToListAsync();
            }
            var genreDict = new Dictionary<string, int>();
            var genreSong = _context.SongGenre;
            foreach (var song in userSong)
            {
                var genre = genreSong.First(a => a.title == song.title);
                if (!genreDict.ContainsKey(genre.name))
                {
                    genreDict.Add(genre.name, song.listen_count);
                }
                else
                {
                    genreDict[genre.name] += song.listen_count;
                }
            }
            
            var top = genreDict.OrderByDescending(kp => kp.Value)
                .Select(kp => kp.Key).Take(amount);
            return top;
        }

        private async Task<IEnumerable<string>> GetTopSongsFriends(string username, int amount)
        {
            var friends = await _context.UsersFollower.Where(uf => uf.follower_username == username)
                .Select(u => u.username)
                .ToListAsync();
            var userSongs = await _context.UserSong.ToListAsync();
            
            var friendSongs = new Dictionary<string, int>();
            foreach (var friend in friends)
            {
                var usersong =  userSongs.Where(us => us.username == friend).ToList();
                var songDict = CreateSongDict(usersong);
                foreach (var kp in songDict)
                {
                    if (!friendSongs.ContainsKey(kp.Key))
                    {
                        friendSongs.Add(kp.Key, kp.Value);
                    }
                    else
                    {
                        friendSongs[kp.Key] += kp.Value;
                    }
                }
            }
            var top = friendSongs.OrderByDescending(kp => kp.Value)
                .Select(kp => kp.Key).Take(amount);
            
            return top;
        }

        private async Task<IEnumerable<string>> GetArtistsOtherSongs(List<string> artistNames)
        {
            var allSongs = new List<string>();
            var artSongs = _context.ArtistsSong;
            foreach (var artist in artistNames)
            {
                allSongs.AddRange(await artSongs.Where(a => a.artist_name == artist)
                    .Select(s => s.title)
                    .ToListAsync());
            }

            return allSongs;
        }
        private async Task<IEnumerable<string>> BuildForYou(string username)
        {
            var friendsSongs =  await GetTopSongsFriends(username, 50);
            var friendsSongsList = friendsSongs.ToList();
            
            var topGenreSongs = await GetTopGenres(username,5);
            var topGenreSongsList = topGenreSongs.ToList();
            var topArtits =  await GetArtistsOtherSongs(GetTopArtists(username, 10));
            var topArtistsList = topArtits.ToList();
            var reccomendAmount = 10;
            var toReturn = new List<string>();
            for (var i = 0; i < reccomendAmount; i++)
            {
                var rand = new Random();
                var listType = rand.Next(0, 3);
                switch (listType)
                {
                    case 0:
                        try
                        {
                            var index = rand.Next(0, friendsSongsList.Count);
                            toReturn.Add(friendsSongsList[index]);
                            friendsSongsList.RemoveAt(index);
                            break;
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                        
                    case 1:
                        try
                        {
                            var index = rand.Next(0, topGenreSongsList.Count);
                            toReturn.Add(topGenreSongsList[index]);
                            topGenreSongsList.RemoveAt(index);
                            break;
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                    default:
                        try
                        {
                            var index = rand.Next(0, topArtistsList.Count);
                            toReturn.Add(topArtistsList[index]);
                            topArtistsList.RemoveAt(index);
                            break;
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                }
            }

            return toReturn;

        }
    }
}