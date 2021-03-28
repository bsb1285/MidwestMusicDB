using System;

namespace MidwestMusicDB.Shared.Models
{
    public class Playlist
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime creation_date { get; set; }
        public int song_count { get; set; }
        public TimeSpan total_duration { get; set; }
    }
}