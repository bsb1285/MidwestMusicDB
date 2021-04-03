namespace MidwestMusicDB.Client.Pages.Shared
{
    public class SearchModel
    {
        public string SearchString { get; set; }
        public string[] searchTypes = new[] {"Song", "Slbum", "Artist", "Genre"};
        public string searchType { get; set; }
    }
}