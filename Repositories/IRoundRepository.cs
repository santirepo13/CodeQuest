using System.Collections.Generic;
using System.Data;
using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones CRUD para rondas
    /// Implementa el patrón Repository
    /// </summary>
    public interface IRoundRepository
    {
        // CREATE operations
        /// <summary>
        /// Crea una nueva ronda para un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>ID de la ronda creada</returns>
        int CreateRound(int userId);

        /// <summary>
        /// Registra una respuesta en una ronda
        /// </summary>
        /// <param name="roundId">ID de la ronda</param>
        /// <param name="questionId">ID de la pregunta</param>
        /// <param name="choiceId">ID de la opción seleccionada</param>
        /// <param name="timeSpentSec">Tiempo gastado en segundos</param>
        void SubmitAnswer(int roundId, int questionId, int choiceId, int timeSpentSec);

        // READ operations
        /// <summary>
        /// Obtiene una ronda por su ID
        /// </summary>
        /// <param name="roundId">ID de la ronda</param>
        /// <returns>Ronda encontrada o null</returns>
        Round GetRoundById(int roundId);

        /// <summary>
        /// Obtiene todas las rondas de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de rondas del usuario</returns>
        List<Round> GetRoundsByUser(int userId);

        /// <summary>
        /// Obtiene el ranking de mejores jugadores
        /// </summary>
        /// <returns>DataTable con el ranking</returns>
        DataTable GetTopRanking();

        // UPDATE operations
        /// <summary>
        /// Cierra una ronda y calcula los resultados
        /// </summary>
        /// <param name="roundId">ID de la ronda a cerrar</param>
        /// <returns>Resultado de la ronda</returns>
        RoundResult CloseRound(int roundId);

        // DELETE operations
        /// <summary>
        /// Elimina una ronda y sus respuestas
        /// </summary>
        /// <param name="roundId">ID de la ronda a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteRound(int roundId);
    }
}