using System.Data;
using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    public interface IRoundRepository
    {
        int CreateRound(int userId);
        void SubmitAnswer(int roundId, int questionId, int choiceId, int timeSpentSec);
        RoundResult CloseRound(int roundId);
        DataTable GetTopRanking();
    }
}