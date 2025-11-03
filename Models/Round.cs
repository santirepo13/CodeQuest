using System;

namespace CodeQuest.Models
{
    public class Round
    {
        public int RoundID { get; set; }
        public int UserID { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Score { get; set; }
        public int XpEarned { get; set; }
        public int DurationSec { get; set; }
    }

    public class RoundResult
    {
        public int Score { get; set; }
        public int XpEarned { get; set; }
        public int Correctas { get; set; }
        public int TiempoTotalSegundos { get; set; }
    }
}