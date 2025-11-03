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
    /// Repositorio para operaciones de usuarios en la base de datos
    /// Implementa IUserRepository (Abstracción) y usa el patrón Singleton para conexiones
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Constructor que usa el Singleton DbConnection
        /// </summary>
        public UserRepository()
        {
            // No necesita parámetros ya que usa el Singleton
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

                using (var connection = DbConnection.GetConnection())
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

                using (var connection = DbConnection.GetConnection())
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
            using (var connection = DbConnection.GetConnection())
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
            using (var connection = DbConnection.GetConnection())
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

        /// <summary>
        /// Actualiza la información de un usuario con validaciones completas
        /// </summary>
        /// <param name="user">Usuario con la información actualizada</param>
        /// <returns>True si se actualizó correctamente</returns>
        /// <exception cref="ArgumentNullException">Se lanza cuando user es null</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool UpdateUser(User user)
        {
            try
            {
                // Validaciones de entrada
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                if (user.UserID <= 0)
                    throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(user));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Users SET Username = @username, Xp = @xp, Level = @level WHERE UserID = @userId", connection))
                    {
                        command.Parameters.AddWithValue("@userId", user.UserID);
                        command.Parameters.AddWithValue("@username", user.Username);
                        command.Parameters.AddWithValue("@xp", user.Xp);
                        command.Parameters.AddWithValue("@level", user.Level);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al actualizar usuario: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al actualizar usuario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario de la base de datos con validaciones
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        /// <exception cref="ArgumentException">Se lanza cuando userId es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool DeleteUser(int userId)
        {
            try
            {
                // Validaciones de entrada
                if (userId <= 0)
                    throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(userId));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("DELETE FROM Users WHERE UserID = @userId", connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al eliminar usuario: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al eliminar usuario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza solo el nombre de usuario usando procedimiento almacenado
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="newUsername">Nuevo nombre de usuario</param>
        /// <returns>True si se actualizó correctamente</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool UpdateUsername(int userId, string newUsername)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(userId));

                if (string.IsNullOrWhiteSpace(newUsername))
                    throw new ArgumentException("El nuevo nombre de usuario no puede estar vacío", nameof(newUsername));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spUser_UpdateUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@NewUsername", newUsername.Trim());
                        
                        using (var reader = command.ExecuteReader())
                        {
                            return reader.HasRows; // Si hay filas, la actualización fue exitosa
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al actualizar nombre: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al actualizar nombre: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina completamente un usuario y todos sus datos usando procedimiento almacenado
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool DeleteUserComplete(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(userId));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spUser_DeleteComplete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userId);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            return reader.HasRows; // Si hay filas, la eliminación fue exitosa
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al eliminar usuario: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al eliminar usuario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Resetea el XP de un usuario a 0 usando procedimiento almacenado
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se reseteó correctamente</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool ResetUserXP(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("El ID del usuario debe ser mayor a 0", nameof(userId));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spUser_ResetXP", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userId);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            return reader.HasRows; // Si hay filas, el reset fue exitoso
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al resetear XP: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al resetear XP: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios de la base de datos
        /// </summary>
        /// <returns>Lista de todos los usuarios</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public List<User> GetAllUsers()
        {
            try
            {
                var users = new List<User>();

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT UserID, Username, Xp, Level, CreatedAt FROM Users ORDER BY Xp DESC", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    UserID = SafeConverter.ToInt32(reader["UserID"]),
                                    Username = reader.GetString("Username"),
                                    Xp = SafeConverter.ToInt32(reader["Xp"]),
                                    Level = SafeConverter.ToInt32(reader["Level"]),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                });
                            }
                        }
                    }
                }

                return users;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al obtener usuarios: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al obtener usuarios: {ex.Message}", ex);
            }
        }
    }
}