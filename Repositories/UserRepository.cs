using System;
using System.Data;
using Microsoft.Data.SqlClient;
using CodeQuest.Factories;
using CodeQuest.Models;
using CodeQuest.Utils;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Repositorio para operaciones de usuarios en la base de datos
    /// Implementa IUserRepository (Abstracción) y manejo de errores con try-catch
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnectionFactory connectionFactory;

        /// <summary>
        /// Constructor que recibe la factory de conexiones (Inyección de Dependencias)
        /// </summary>
        /// <param name="connectionFactory">Factory para crear conexiones a la base de datos</param>
        /// <exception cref="ArgumentNullException">Se lanza cuando connectionFactory es null</exception>
        public UserRepository(IDatabaseConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        /// <summary>
        /// Crea un nuevo usuario en la base de datos con validaciones completas
        /// </summary>
        /// <param name="username">Nombre de usuario a crear</param>
        /// <returns>ID del usuario creado</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el username es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public int CreateUser(string username)
        {
            try
            {
                // Validaciones de entrada
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));
                
                if (username.Length < 3)
                    throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres", nameof(username));
                
                if (username.Length > 50)
                    throw new ArgumentException("El nombre de usuario no puede exceder 50 caracteres", nameof(username));

                username = username.Trim();

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
                        throw new InvalidOperationException("No se pudo crear el usuario - resultado nulo de la base de datos");
                    }
                }
            }
            catch (ArgumentException)
            {
                // Re-lanza las excepciones de validación
                throw;
            }
            catch (SqlException sqlEx)
            {
                // Manejo específico de errores de SQL Server
                if (sqlEx.Number == 2627) // Violación de clave única
                    throw new InvalidOperationException($"El nombre de usuario '{username}' ya existe", sqlEx);
                
                throw new InvalidOperationException($"Error de base de datos al crear usuario: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Manejo general de errores
                throw new InvalidOperationException($"Error inesperado al crear usuario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica si un usuario existe en la base de datos con manejo de errores
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si el usuario existe, False en caso contrario</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el username es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool UserExists(string username)
        {
            try
            {
                // Validaciones de entrada
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));

                username = username.Trim();

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
            catch (ArgumentException)
            {
                // Re-lanza las excepciones de validación
                throw;
            }
            catch (SqlException sqlEx)
            {
                // Manejo específico de errores de SQL Server
                throw new InvalidOperationException($"Error de base de datos al verificar usuario: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Manejo general de errores
                throw new InvalidOperationException($"Error inesperado al verificar usuario: {ex.Message}", ex);
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