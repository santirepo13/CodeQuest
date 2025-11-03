using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CodeQuest.Database;
using CodeQuest.Models;
using CodeQuest.Utils;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Repositorio para operaciones de preguntas usando el patrón Singleton para conexiones
    /// </summary>
    public class QuestionRepository : IQuestionRepository
    {
        /// <summary>
        /// Constructor que usa el Singleton DbConnection
        /// </summary>
        public QuestionRepository()
        {
            // No necesita parámetros ya que usa el Singleton
        }

        public List<Question> GetQuestionsByDifficulty(int difficulty, int count = 3)
        {
            var questions = new List<Question>();
            
            using (var connection = DbConnection.GetConnection())
            {
                connection.Open();
                
                string query = $@"
                    SELECT TOP {count} QuestionID, Text, Difficulty 
                    FROM Questions 
                    WHERE Difficulty = @difficulty 
                    ORDER BY NEWID()";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@difficulty", difficulty);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new Question
                            {
                                QuestionID = reader.GetInt32(0),
                                Text = reader.GetString(1),
                                Difficulty = SafeConverter.ToInt32(reader[2])
                            });
                        }
                    }
                }
            }
            
            
            foreach (var question in questions)
            {
                question.Choices = GetChoicesForQuestion(question.QuestionID);
            }
            
            return questions;
        }

        public List<Choice> GetChoicesForQuestion(int questionId)
        {
            var choices = new List<Choice>();
            
            using (var connection = DbConnection.GetConnection())
            {
                connection.Open();
                
                string query = "SELECT ChoiceID, ChoiceText, IsCorrect FROM Choices WHERE QuestionID = @questionId ORDER BY NEWID()";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@questionId", questionId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            choices.Add(new Choice
                            {
                                ChoiceID = reader.GetInt32(0),
                                ChoiceText = reader.GetString(1),
                                IsCorrect = reader.GetBoolean(2)
                            });
                        }
                    }
                }
            }
            
            return choices;
        }

        /// <summary>
        /// Obtiene una pregunta específica por su ID
        /// </summary>
        /// <param name="questionId">ID de la pregunta</param>
        /// <returns>Pregunta encontrada o null</returns>
        public Question GetQuestionById(int questionId)
        {
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    
                    string query = "SELECT QuestionID, Text, Difficulty FROM Questions WHERE QuestionID = @questionId";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@questionId", questionId);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var question = new Question
                                {
                                    QuestionID = reader.GetInt32(0),
                                    Text = reader.GetString(1),
                                    Difficulty = SafeConverter.ToInt32(reader[2])
                                };
                                
                                // Cerrar el reader antes de obtener las opciones
                                reader.Close();
                                
                                // Obtener las opciones
                                question.Choices = GetChoicesForQuestion(questionId);
                                
                                return question;
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener pregunta: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las preguntas de la base de datos
        /// </summary>
        /// <returns>Lista de todas las preguntas</returns>
        public List<Question> GetAllQuestions()
        {
            try
            {
                var questions = new List<Question>();
                
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    
                    string query = "SELECT QuestionID, Text, Difficulty FROM Questions ORDER BY Difficulty, QuestionID";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                questions.Add(new Question
                                {
                                    QuestionID = reader.GetInt32(0),
                                    Text = reader.GetString(1),
                                    Difficulty = SafeConverter.ToInt32(reader[2])
                                });
                            }
                        }
                    }
                    
                    // Obtener opciones para cada pregunta
                    foreach (var question in questions)
                    {
                        question.Choices = GetChoicesForQuestion(question.QuestionID);
                    }
                }
                
                return questions;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener todas las preguntas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea una nueva pregunta en la base de datos
        /// </summary>
        /// <param name="question">Pregunta a crear</param>
        /// <returns>ID de la pregunta creada</returns>
        public int CreateQuestion(Question question)
        {
            try
            {
                if (question == null)
                    throw new ArgumentNullException(nameof(question));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spQuestion_New", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Text", question.Text);
                        command.Parameters.AddWithValue("@Difficulty", question.Difficulty);
                        
                        var result = command.ExecuteScalar();
                        return SafeConverter.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al crear pregunta: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea una nueva opción para una pregunta
        /// </summary>
        /// <param name="questionId">ID de la pregunta</param>
        /// <param name="choice">Opción a crear</param>
        /// <returns>ID de la opción creada</returns>
        public int CreateChoice(int questionId, Choice choice)
        {
            try
            {
                if (choice == null)
                    throw new ArgumentNullException(nameof(choice));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spOption_new", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@QuestionID", questionId);
                        command.Parameters.AddWithValue("@ChoiceText", choice.ChoiceText);
                        command.Parameters.AddWithValue("@IsCorrect", choice.IsCorrect);
                        
                        var result = command.ExecuteScalar();
                        return SafeConverter.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al crear opción: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza una pregunta existente
        /// </summary>
        /// <param name="question">Pregunta con datos actualizados</param>
        /// <returns>True si se actualizó correctamente</returns>
        public bool UpdateQuestion(Question question)
        {
            try
            {
                if (question == null)
                    throw new ArgumentNullException(nameof(question));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Questions SET Text = @text, Difficulty = @difficulty WHERE QuestionID = @questionId", connection))
                    {
                        command.Parameters.AddWithValue("@questionId", question.QuestionID);
                        command.Parameters.AddWithValue("@text", question.Text);
                        command.Parameters.AddWithValue("@difficulty", question.Difficulty);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al actualizar pregunta: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina una pregunta y todas sus opciones
        /// </summary>
        /// <param name="questionId">ID de la pregunta a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        public bool DeleteQuestion(int questionId)
        {
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    
                    // Primero eliminar las opciones (por la restricción de clave foránea)
                    using (var command = new SqlCommand("DELETE FROM Choices WHERE QuestionID = @questionId", connection))
                    {
                        command.Parameters.AddWithValue("@questionId", questionId);
                        command.ExecuteNonQuery();
                    }
                    
                    // Luego eliminar la pregunta
                    using (var command = new SqlCommand("DELETE FROM Questions WHERE QuestionID = @questionId", connection))
                    {
                        command.Parameters.AddWithValue("@questionId", questionId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar pregunta: {ex.Message}", ex);
            }
        }
    }
}