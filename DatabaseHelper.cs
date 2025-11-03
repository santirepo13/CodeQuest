using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CodeQuest
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = @"Server=DESKTOP-FN66L1D\SQLEXPRESS;Database=CodeQuest;Integrated Security=true;TrustServerCertificate=true;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static int CreateUser(string username)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spUser_New", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", username);
                    
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public static bool UserExists(string username)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static int GetUserId(string username)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT UserID FROM Users WHERE Username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}