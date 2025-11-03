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

        // Admin operations for ranking
        /// <summary>
        /// Actualiza el nombre de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="newUsername">Nuevo nombre</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateUsername(int userId, string newUsername);

        /// <summary>
        /// Elimina completamente un usuario del ranking
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteUserFromRanking(int userId);

        /// <summary>
        /// Resetea el XP de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se reseteó correctamente</returns>
        bool ResetUserXP(int userId);
    }
}