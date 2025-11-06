using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CodeQuest.Database;
using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Repositorio para operaciones de administradores en la base de datos
    /// Implementa IAdministratorRepository (Abstracción) y usa el patrón Singleton para conexiones
    /// </summary>
    public class AdministratorRepository : IAdministratorRepository
    {
        /// <summary>
        /// Constructor que usa el Singleton DbConnection
        /// </summary>
        public AdministratorRepository()
        {
            // No necesita parámetros ya que usa el Singleton
        }

        /// <summary>
        /// Crea un nuevo administrador en la base de datos con validaciones completas
        /// </summary>
        /// <param name="username">Nombre de usuario a crear</param>
        /// <param name="password">Contraseña del administrador</param>
        /// <returns>ID del administrador creado</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el username o password son inválidos</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public int CreateAdministrator(string username, string password)
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

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));
                
                if (password.Length < 4)
                    throw new ArgumentException("La contraseña debe tener al menos 4 caracteres", nameof(password));

                username = username.Trim();

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spAdministrator_Create", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        
                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        throw new InvalidOperationException("No se pudo crear el administrador - resultado nulo de la base de datos");
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
                
                throw new InvalidOperationException($"Error de base de datos al crear administrador: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Manejo general de errores
                throw new InvalidOperationException($"Error inesperado al crear administrador: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica si un administrador existe en la base de datos con manejo de errores
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si el administrador existe, False en caso contrario</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el username es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool AdministratorExists(string username)
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
                    using (var command = new SqlCommand("SELECT COUNT(*) FROM Administrators WHERE Username = @username", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        var result = command.ExecuteScalar();
                        int count = Convert.ToInt32(result);
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
                throw new InvalidOperationException($"Error de base de datos al verificar administrador: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Manejo general de errores
                throw new InvalidOperationException($"Error inesperado al verificar administrador: {ex.Message}", ex);
            }
        }

        public int GetAdministratorId(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));

                username = username.Trim();

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT AdminID FROM Administrators WHERE Username = @username", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        var result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        return 0;
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al obtener ID de administrador: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al obtener ID de administrador: {ex.Message}", ex);
            }
        }

        public Administrator GetAdministratorById(int adminId)
        {
            try
            {
                if (adminId <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(adminId));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT AdminID, Username, Password, CreatedAt FROM Administrators WHERE AdminID = @adminId", connection))
                    {
                        command.Parameters.AddWithValue("@adminId", adminId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Administrator
                                {
                                    AdminID = Convert.ToInt32(reader["AdminID"]),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al obtener administrador: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al obtener administrador: {ex.Message}", ex);
            }
        }

        public Administrator Authenticate(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));
                
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

                username = username.Trim();

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spAdministrator_Login", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Administrator
                                {
                                    AdminID = Convert.ToInt32(reader["AdminID"]),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al autenticar administrador: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al autenticar administrador: {ex.Message}", ex);
            }
        }

        public bool UpdateAdministrator(Administrator administrator)
        {
            try
            {
                // Validaciones de entrada
                if (administrator == null)
                    throw new ArgumentNullException(nameof(administrator));
                
                if (administrator.AdminID <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(administrator));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Administrators SET Username = @username, Password = @password WHERE AdminID = @adminId", connection))
                    {
                        command.Parameters.AddWithValue("@adminId", administrator.AdminID);
                        command.Parameters.AddWithValue("@username", administrator.Username);
                        command.Parameters.AddWithValue("@password", administrator.Password);
                        
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
                throw new InvalidOperationException($"Error de base de datos al actualizar administrador: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al actualizar administrador: {ex.Message}", ex);
            }
        }

        public bool DeleteAdministrator(int adminId)
        {
            try
            {
                // Validaciones de entrada
                if (adminId <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(adminId));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("DELETE FROM Administrators WHERE AdminID = @adminId", connection))
                    {
                        command.Parameters.AddWithValue("@adminId", adminId);
                        
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
                throw new InvalidOperationException($"Error de base de datos al eliminar administrador: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al eliminar administrador: {ex.Message}", ex);
            }
        }

        public List<Administrator> GetAllAdministrators()
        {
            try
            {
                var administrators = new List<Administrator>();

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT AdminID, Username, Password, CreatedAt FROM Administrators ORDER BY CreatedAt DESC", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                administrators.Add(new Administrator
                                {
                                    AdminID = Convert.ToInt32(reader["AdminID"]),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                });
                            }
                        }
                    }
                }

                return administrators;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al obtener administradores: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al obtener administradores: {ex.Message}", ex);
            }
        }

        public bool UpdatePassword(int adminId, string newPassword)
        {
            try
            {
                if (adminId <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(adminId));
                
                if (string.IsNullOrWhiteSpace(newPassword))
                    throw new ArgumentException("La nueva contraseña no puede estar vacía", nameof(newPassword));

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("spAdministrator_UpdatePassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminID", adminId);
                        command.Parameters.AddWithValue("@NewPassword", newPassword);
                        
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
                throw new InvalidOperationException($"Error de base de datos al actualizar contraseña: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al actualizar contraseña: {ex.Message}", ex);
            }
        }

        public Administrator GetAdministratorByUsername(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));

                username = username.Trim();

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT AdminID, Username, Password, CreatedAt FROM Administrators WHERE Username = @username", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Administrator
                                {
                                    AdminID = Convert.ToInt32(reader["AdminID"]),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException($"Error de base de datos al obtener administrador por username: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al obtener administrador por username: {ex.Message}", ex);
            }
        }
    }
}