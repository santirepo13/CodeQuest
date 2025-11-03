using System;
using System.Data;
using Microsoft.Data.SqlClient;
using CodeQuest.Factories;
using CodeQuest.Models;
using CodeQuest.Utils;

namespace CodeQuest.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnectionFactory connectionFactory;

        public UserRepository(IDatabaseConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public int CreateUser(string username)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spUser_New", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", username);
                    
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return SafeConverter.ToInt32(result);
                    }
                    throw new Exception("No se pudo crear el usuario");
                }
            }
        }

        public bool UserExists(string username)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var result = command.ExecuteScalar();
                    int count = SafeConverter.ToInt32(result);
                    return count > 0;
                }
            }
        }

        public int GetUserId(string username)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT UserID FROM Users WHERE Username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return SafeConverter.ToInt32(result);
                    }
                    return 0;
                }
            }
        }

        public User GetUserById(int userId)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT UserID, Username, Xp, Level, CreatedAt FROM Users WHERE UserID = @userId", connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = SafeConverter.ToInt32(reader["UserID"]),
                                Username = reader.GetString("Username"),
                                Xp = SafeConverter.ToInt32(reader["Xp"]),
                                Level = SafeConverter.ToInt32(reader["Level"]),
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}