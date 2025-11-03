using System;
using System.Data;
using Microsoft.Data.SqlClient;
using CodeQuest.Factories;
using CodeQuest.Models;
using CodeQuest.Utils;

namespace CodeQuest.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private readonly IDatabaseConnectionFactory connectionFactory;

        public RoundRepository(IDatabaseConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public int CreateRound(int userId)
        {
            using (var connection = connectionFactory.CreateConnection())
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
            using (var connection = connectionFactory.CreateConnection())
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
            using (var connection = connectionFactory.CreateConnection())
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
            using (var connection = connectionFactory.CreateConnection())
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
    }
}