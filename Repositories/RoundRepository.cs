using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CodeQuest.Database;
using CodeQuest.Models;
using CodeQuest.Utils;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Repositorio para operaciones de rondas usando el patrón Singleton para conexiones
    /// </summary>
    public class RoundRepository : IRoundRepository
    {
        /// <summary>
        /// Constructor que usa el Singleton DbConnection
        /// </summary>
        public RoundRepository()
        {
            // No necesita parámetros ya que usa el Singleton
        }

        public int CreateRound(int userId)
        {
            using (var connection = DbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spRounds_New", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);
                    
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return SafeConverter.ToInt32(result);
                    }
                    throw new Exception("No se pudo crear la ronda");
                }
            }
        }

        public void SubmitAnswer(int roundId, int questionId, int choiceId, int timeSpentSec)
        {
            using (var connection = DbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spRounds_Answer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoundID", roundId);
                    command.Parameters.AddWithValue("@QuestionID", questionId);
                    command.Parameters.AddWithValue("@ChoiceID", choiceId);
                    command.Parameters.AddWithValue("@TimeSpentSec", timeSpentSec);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public RoundResult CloseRound(int roundId)
        {
            using (var connection = DbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spRounds_Close", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoundID", roundId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RoundResult
                            {
                                Score = SafeConverter.ToInt32(reader["Score"]),
                                XpEarned = SafeConverter.ToInt32(reader["XpEarned"]),
                                Correctas = SafeConverter.ToInt32(reader["Correctas"]),
                                TiempoTotalSegundos = SafeConverter.ToInt32(reader["TiempoTotalSegundos"])
                            };
                        }
                    }
                }
            }
            
            return null;
        }

        public DataTable GetTopRanking()
        {
            using (var connection = DbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spUsers_TopRanking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    var adapter = new SqlDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    
                    return dataTable;
                }
            }
        }

        /// <summary>
        /// Obtiene una ronda específica por su ID
        /// </summary>
        /// <param name="roundId">ID de la ronda</param>
        /// <returns>Ronda encontrada o null</returns>
        public Round GetRoundById(int roundId)
        {
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT RoundID, UserID, StartedAt, CompletedAt, Score, XpEarned, DurationSec FROM Rounds WHERE RoundID = @roundId", connection))
                    {
                        command.Parameters.AddWithValue("@roundId", roundId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Round
                                {
                                    RoundID = SafeConverter.ToInt32(reader["RoundID"]),
                                    UserID = SafeConverter.ToInt32(reader["UserID"]),
                                    StartedAt = reader.GetDateTime("StartedAt"),
                                    CompletedAt = reader.IsDBNull("CompletedAt") ? null : reader.GetDateTime("CompletedAt"),
                                    Score = SafeConverter.ToInt32(reader["Score"]),
                                    XpEarned = SafeConverter.ToInt32(reader["XpEarned"]),
                                    DurationSec = SafeConverter.ToInt32(reader["DurationSec"])
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener ronda: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las rondas de un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de rondas del usuario</returns>
        public List<Round> GetRoundsByUser(int userId)
        {
            try
            {
                var rounds = new List<Round>();

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT RoundID, UserID, StartedAt, CompletedAt, Score, XpEarned, DurationSec FROM Rounds WHERE UserID = @userId ORDER BY StartedAt DESC", connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rounds.Add(new Round
                                {
                                    RoundID = SafeConverter.ToInt32(reader["RoundID"]),
                                    UserID = SafeConverter.ToInt32(reader["UserID"]),
                                    StartedAt = reader.GetDateTime("StartedAt"),
                                    CompletedAt = reader.IsDBNull("CompletedAt") ? null : reader.GetDateTime("CompletedAt"),
                                    Score = SafeConverter.ToInt32(reader["Score"]),
                                    XpEarned = SafeConverter.ToInt32(reader["XpEarned"]),
                                    DurationSec = SafeConverter.ToInt32(reader["DurationSec"])
                                });
                            }
                        }
                    }
                }

                return rounds;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener rondas del usuario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina una ronda y todas sus respuestas asociadas
        /// </summary>
        /// <param name="roundId">ID de la ronda a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        public bool DeleteRound(int roundId)
        {
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    
                    // Primero eliminar las respuestas (por la restricción de clave foránea)
                    using (var command = new SqlCommand("DELETE FROM RoundAnswers WHERE RoundID = @roundId", connection))
                    {
                        command.Parameters.AddWithValue("@roundId", roundId);
                        command.ExecuteNonQuery();
                    }
                    
                    // Luego eliminar la ronda
                    using (var command = new SqlCommand("DELETE FROM Rounds WHERE RoundID = @roundId", connection))
                    {
                        command.Parameters.AddWithValue("@roundId", roundId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar ronda: {ex.Message}", ex);
            }
        }
    }
}