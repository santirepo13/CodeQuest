using System;

namespace CodeQuest.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}