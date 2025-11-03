using System.Collections.Generic;
using System.Data;
using CodeQuest.Models;

namespace CodeQuest.Services
{
    public interface IGameService
    {
        // User operations
        int CreateUser(string username);
        bool UserExists(string username);
        int GetUserId(string username);
        User GetUserById(int userId);

        // Game operations
        int StartNewRound(int userId);
        List<Question> GetQuestionsForRound(int difficulty);
        void SubmitAnswer(int roundId, int questionId, int choiceId, int timeSpentSec);
        RoundResult CompleteRound(int roundId);
        
        // Statistics
        DataTable GetTopRanking();
        User GetUserStats(int userId);
    }
}