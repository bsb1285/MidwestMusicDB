using System;

namespace MidwestMusicDB.Shared.Models
{
    public class Song
    {
        public string title { get; set; }
        public TimeSpan length { get; set; }
        public DateTime song_release_date { get; set; }
    }
}