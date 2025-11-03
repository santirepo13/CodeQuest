using System.Collections.Generic;

namespace CodeQuest.Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public int Difficulty { get; set; }
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    public class Choice
    {
        public int ChoiceID { get; set; }
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }
}