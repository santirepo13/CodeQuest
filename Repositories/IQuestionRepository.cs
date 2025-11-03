using System.Collections.Generic;
using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones CRUD para preguntas
    /// Implementa el patrón Repository
    /// </summary>
    public interface IQuestionRepository
    {
        // READ operations
        /// <summary>
        /// Obtiene preguntas por dificultad
        /// </summary>
        /// <param name="difficulty">Nivel de dificultad (1-3)</param>
        /// <param name="count">Cantidad de preguntas a obtener</param>
        /// <returns>Lista de preguntas</returns>
        List<Question> GetQuestionsByDifficulty(int difficulty, int count = 3);

        /// <summary>
        /// Obtiene las opciones de una pregunta específica
        /// </summary>
        /// <param name="questionId">ID de la pregunta</param>
        /// <returns>Lista de opciones</returns>
        List<Choice> GetChoicesForQuestion(int questionId);

        /// <summary>
        /// Obtiene una pregunta por su ID
        /// </summary>
        /// <param name="questionId">ID de la pregunta</param>
        /// <returns>Pregunta encontrada o null</returns>
        Question GetQuestionById(int questionId);

        /// <summary>
        /// Obtiene todas las preguntas
        /// </summary>
        /// <returns>Lista de todas las preguntas</returns>
        List<Question> GetAllQuestions();

        // CREATE operations
        /// <summary>
        /// Crea una nueva pregunta
        /// </summary>
        /// <param name="question">Pregunta a crear</param>
        /// <returns>ID de la pregunta creada</returns>
        int CreateQuestion(Question question);

        /// <summary>
        /// Crea una nueva opción para una pregunta
        /// </summary>
        /// <param name="questionId">ID de la pregunta</param>
        /// <param name="choice">Opción a crear</param>
        /// <returns>ID de la opción creada</returns>
        int CreateChoice(int questionId, Choice choice);

        // UPDATE operations
        /// <summary>
        /// Actualiza una pregunta existente
        /// </summary>
        /// <param name="question">Pregunta con datos actualizados</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateQuestion(Question question);

        // DELETE operations
        /// <summary>
        /// Elimina una pregunta y sus opciones
        /// </summary>
        /// <param name="questionId">ID de la pregunta a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteQuestion(int questionId);
    }
}