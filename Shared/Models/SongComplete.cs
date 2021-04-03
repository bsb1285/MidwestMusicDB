namespace MidwestMusicDB.Shared.Models
{
    public class SongComplete
    {
        public Song song { get; set; }
        public Album album { get; set; }
        public Artist artist { get; set; }
        public int trackNumber { get; set; }
        public Genre genre { get; set; }
        public string key { get; set; }
        
        
        public SongComplete()
        {
            
        }

        public void SetKey()
        {
            key = song.title + album.album_name + artist.artist_name + genre.name;
        }

        public bool ContainedInSearch(string searchString)
        {
            return song.title.Contains(searchString) || album.album_name.Contains(searchString)
                                                     || artist.artist_name.Contains(searchString)
                                                     || genre.name.Contains(searchString);
        }
    }
}