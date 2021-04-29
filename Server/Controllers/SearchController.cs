using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        
        public SearchController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Song.ToListAsync());
        }

        [HttpGet("{searchString}/{searchType}")]
        public async Task<IActionResult> Get(string searchString, string searchType)
        {
            Console.WriteLine("Start Search");
            switch (searchType)
            {
                case "song":
                {
                    var songs = await _context.Song.ToListAsync();
                    var search_songs = new List<Song>();
                    var songs_new = from s in songs
                        select s;
        
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        search_songs.AddRange(songs_new
                            .Where(s => s.title.Contains(searchString)));
                        //Console.WriteLine(songs_new);
                    }
                    Console.WriteLine("End Song search");
                    
                    return Ok(await BuildCompleteSong(search_songs));
                }
                case "artist":
                {
                    var artists = await _context.Artist.ToListAsync();
                    var search_artists = artists.Where(a => a.artist_name.Contains(searchString))
                        .Select(a_ => a_.artist_name).ToList();
                    //var art_songs = _context.ArtistsSong.Select(a_s =>);
                    var art_songs = from art_song in _context.ArtistsSong
                        where search_artists.Contains(art_song.artist_name)
                        select art_song.title;

                    var songFromArtist = from song in _context.Song
                        where art_songs.Contains(song.title)
                        select song;
                    Console.WriteLine("End Song search");
                    return Ok(songFromArtist);
                }
                case "album":
                {
                    var albums = await _context.Album.ToListAsync();
                    var search_albums = albums.Where(a => a.album_name.Contains(searchString))
                        .Select(al => al.album_name);
                    var alb_songs = from alb_song in _context.SongOnAlbum
                        where search_albums.Contains(alb_song.title)
                        select alb_song.title;
                    var songFromAlbums = from songFromAlbum in _context.Song
                        where alb_songs.Contains(songFromAlbum.title)
                        select songFromAlbum;
                    Console.WriteLine("End Song search");
                    return Ok(songFromAlbums);
                }
                case "genre":
                {
                    var genres = await _context.Genre.ToListAsync();
                    var search_genres = genres.Where(g => g.name.Contains(searchString)).Select(ge => ge.name);
                    var gen_songs = from gen_song in _context.SongGenre
                        where search_genres.Contains(gen_song.name)
                        select gen_song.title;
                    var songsFromGenres = from songsFromGenre in _context.Song
                        where gen_songs.Contains(songsFromGenre.title)
                        select songsFromGenre;
                    Console.WriteLine("End Song search");
                    return Ok(songsFromGenres);
                }
                case "user":
                {
                    var users = await _context.Users.ToListAsync();
                    var user_search = users.Where(u => u.email.Contains(searchString));
                    
                    Console.WriteLine("End user search");
                    return Ok(user_search);
                }
                default:
                {
                    var users = await _context.Users.ToListAsync();
                    var user_search = users.Where(u => u.email.Contains(searchString));
                    var userCompleteList = new List<UserComplete>();
                    foreach (var user in user_search)
                    {
                        var userFollower = await _context.UsersFollower.ToListAsync();
                        if (userFollower.Exists(uf =>
                            uf.follower_username.Equals(searchType) && uf.username.Equals(user.username)))
                        {
                            userCompleteList.Add(new UserComplete() {user = user, isFollowed = true});
                        }
                        else
                        {
                            userCompleteList.Add(new UserComplete() {user = user, isFollowed = false});
                        }
                    }

                    return Ok(userCompleteList);
                }
            }
        }

        [HttpGet("{searchString}/{searchType}/{username}")]
        public async Task<IActionResult> Get(string searchString, string searchType, string username)
        {
            switch (searchType)
            {
                case "song":
                {
                    var songs = await _context.Song.ToListAsync();
                    var search_songs = new List<Song>();
                    var songs_new = from s in songs
                        select s;
                    

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        search_songs.AddRange(songs_new
                            .Where(s => s.title.ToLower().Contains(searchString.ToLower())));

                    }

                    Console.WriteLine("End Song search");

                    return Ok(await BuildCompleteSong(search_songs, username));
                }
                case "artist":
                {
                    var artists = await _context.Artist.ToListAsync();
                    var search_artists = artists.Where(a => a.artist_name.Contains(searchString))
                        .Select(a_ => a_.artist_name).ToList();
                    
                    var art_songs = from art_song in _context.ArtistsSong
                        where search_artists.Contains(art_song.artist_name)
                        select art_song.title;

                    var songFromArtist = from song in _context.Song
                        where art_songs.Contains(song.title)
                        select song;

                    Console.WriteLine("End Song search");
                    return Ok(await BuildCompleteSong(songFromArtist.ToList(), username));
                }
                case "album":
                {
                    var albums = await _context.Album.ToListAsync();
                    var search_albums = albums.Where(a => a.album_name.Contains(searchString))
                        .Select(al => al.album_name);
                    var alb_songs = from alb_song in _context.SongOnAlbum
                        where search_albums.Contains(alb_song.title)
                        select alb_song.title;
                    var songFromAlbums = from songFromAlbum in _context.Song
                        where alb_songs.Contains(songFromAlbum.title)
                        select songFromAlbum;
                    Console.WriteLine("End Song search");
                    return Ok(await BuildCompleteSong(songFromAlbums.ToList(), username));
                }
                case "genre":
                {
                    var genres = await _context.Genre.ToListAsync();
                    var search_genres = genres.Where(g => g.name.Contains(searchString)).Select(ge => ge.name);
                    var gen_songs = from gen_song in _context.SongGenre
                        where search_genres.Contains(gen_song.name)
                        select gen_song.title;
                    var songsFromGenres = from songsFromGenre in _context.Song
                        where gen_songs.Contains(songsFromGenre.title)
                        select songsFromGenre;
                    Console.WriteLine("End Song search");
                    return Ok(await BuildCompleteSong(songsFromGenres.ToList(), username));
                }
                default:
                    return Ok();
            }
            
            
        }

        private async Task<List<SongComplete>> BuildCompleteSong(List<Song> searchSongs, string username = null)
        {
            Console.WriteLine(username);
            var completeSongs = new List<SongComplete>();
            foreach (Song s in searchSongs)
            {

                var artist_song = await _context.ArtistsSong.FirstAsync(song => song.title == s.title);
                var artist = artist_song.artist_name;
                var albums = await _context.SongOnAlbum.FirstAsync(song => song.title == s.title);
                var album = albums.album_name;
                var trackList = await _context.UserSong.ToListAsync();
                var track = -1;
                try
                {
                    track = trackList.Single(us => us.title.Equals(s.title) && us.username.Equals(username))
                        .listen_count;
                }
                catch
                {
                    
                }

                if (track == -1)
                {
                    track = 0;
                    
                }
                completeSongs.Add(new SongComplete(){album = album, artist = artist, listenCount = track, song = s});
            }

            return completeSongs;
        }
    }
    
    
}