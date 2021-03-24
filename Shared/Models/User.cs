using System;


namespace MidwestMusicDB.Shared.Models
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string first_name {get; set; }
        public string last_name {get; set; }
        public DateTime last_access_date { get; set; }
        public DateTime signup_date { get; set; }
        public string email { get; set; }
        
    }
}