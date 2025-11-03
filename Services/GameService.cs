using System.Collections.Generic;
using System.Data;
using CodeQuest.Models;
using CodeQuest.Repositories;

namespace CodeQuest.Services
{
    /// <summary>
    /// Servicio principal del juego que coordina todas las operaciones
    /// Usa el patrón Singleton para conexiones a través de los repositorios
    /// </summary>
    public class GameService : IGameService
    {
        private readonly IUserRepository userRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IRoundRepository roundRepository;

        /// <summary>
        /// Constructor que inicializa los repositorios (ahora sin factory)
        /// </summary>
        public GameService()
        {
            // Los repositorios ahora usan el Singleton internamente
            this.userRepository = new UserRepository();
            this.questionRepository = new QuestionRepository();
            this.roundRepository = new RoundRepository();
        }

        public int CreateUser(string username)
        {
            return userRepository.CreateUser(username);
        }

        public bool UserExists(string username)
        {
            return userRepository.UserExists(username);
        }

        public int GetUserId(string username)
        {
            return userRepository.GetUserId(username);
        }

        public User GetUserById(int userId)
        {
            return userRepository.GetUserById(userId);
        }

        public int StartNewRound(int userId)
        {
            return roundRepository.CreateRound(userId);
        }

        public List<Question> GetQuestionsForRound(int difficulty)
        {
            return questionRepository.GetQuestionsByDifficulty(difficulty, 3);
        }

        public void SubmitAnswer(int roundId, int questionId, int choiceId, int timeSpentSec)
        {
            roundRepository.SubmitAnswer(roundId, questionId, choiceId, timeSpentSec);
        }

        public RoundResult CompleteRound(int roundId)
        {
            return roundRepository.CloseRound(roundId);
        }

        public DataTable GetTopRanking()
        {
            return roundRepository.GetTopRanking();
        }

        public User GetUserStats(int userId)
        {
            return userRepository.GetUserById(userId);
        }

        // Métodos administrativos para el ranking
        /// <summary>
        /// Actualiza el nombre de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="newUsername">Nuevo nombre</param>
        /// <returns>True si se actualizó correctamente</returns>
        public bool UpdateUsername(int userId, string newUsername)
        {
            return userRepository.UpdateUsername(userId, newUsername);
        }

        /// <summary>
        /// Elimina completamente un usuario del ranking
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        public bool DeleteUserFromRanking(int userId)
        {
            return userRepository.DeleteUserComplete(userId);
        }

        /// <summary>
        /// Resetea el XP de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se reseteó correctamente</returns>
        public bool ResetUserXP(int userId)
        {
            return userRepository.ResetUserXP(userId);
        }
    }
}