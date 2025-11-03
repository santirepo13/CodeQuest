using System.Collections.Generic;
using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    public interface IQuestionRepository
    {
        List<Question> GetQuestionsByDifficulty(int difficulty, int count = 3);
        List<Choice> GetChoicesForQuestion(int questionId);
    }
}