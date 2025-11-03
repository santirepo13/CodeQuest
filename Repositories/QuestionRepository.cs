using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CodeQuest.Factories;
using CodeQuest.Models;
using CodeQuest.Utils;

namespace CodeQuest.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IDatabaseConnectionFactory connectionFactory;

        public QuestionRepository(IDatabaseConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public List<Question> GetQuestionsByDifficulty(int difficulty, int count = 3)
        {
            var questions = new List<Question>();
            
            using (var connection = connectionFactory.CreateConnection())
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
            
            using (var connection = connectionFactory.CreateConnection())
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
    }
}