using System;
using Microsoft.Data.SqlClient;

namespace CodeQuest.Database
{
    /// <summary>
    /// Clase Singleton para manejo de conexiones a la base de datos
    /// Implementa el patrón Singleton para garantizar una única instancia de conexión
    /// </summary>
    public sealed class DbConnection
    {
        #region Singleton Implementation
        
        private static DbConnection _instance;
        private static readonly object _lock = new object();
        
        /// <summary>
        /// Instancia única del Singleton (thread-safe)
        /// </summary>
        public static DbConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DbConnection();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Private Fields and Constructor

        private readonly string _connectionString;

        /// <summary>
        /// Constructor privado para prevenir instanciación externa (patrón Singleton)
        /// </summary>
        private DbConnection()
        {
            try
            {
                _connectionString = @"Server=DESKTOP-FN66L1D\SQLEXPRESS;Database=CodeQuest;Integrated Security=true;TrustServerCertificate=true;";
                
                // Validar la cadena de conexión al crear la instancia
                ValidateConnectionString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al inicializar DbConnection: {ex.Message}", ex);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Crea una nueva conexión SQL Server usando la cadena de conexión del Singleton
        /// </summary>
        /// <returns>Nueva instancia de SqlConnection</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas con la conexión</exception>
        public SqlConnection CreateConnection()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al crear conexión: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prueba la conexión a la base de datos
        /// </summary>
        /// <returns>True si la conexión es exitosa, False en caso contrario</returns>
        public bool TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    
                    // Ejecutar una consulta simple para verificar la conexión
                    using (var command = new SqlCommand("SELECT 1", connection))
                    {
                        var result = command.ExecuteScalar();
                        return result != null && Convert.ToInt32(result) == 1;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene información sobre la base de datos conectada
        /// </summary>
        /// <returns>Información de la base de datos</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando no se puede obtener la información</exception>
        public string GetDatabaseInfo()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    
                    using (var command = new SqlCommand("SELECT @@VERSION, DB_NAME()", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string version = reader.GetString(0);
                                string dbName = reader.GetString(1);
                                return $"Base de datos: {dbName}, Versión: {version.Substring(0, Math.Min(50, version.Length))}...";
                            }
                        }
                    }
                }
                
                return "Información no disponible";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener información de la base de datos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene la cadena de conexión (solo para propósitos de debugging)
        /// </summary>
        /// <returns>Cadena de conexión sin información sensible</returns>
        public string GetConnectionStringInfo()
        {
            try
            {
                // Retorna información de la conexión sin datos sensibles
                var builder = new SqlConnectionStringBuilder(_connectionString);
                return $"Servidor: {builder.DataSource}, Base de datos: {builder.InitialCatalog}";
            }
            catch (Exception ex)
            {
                return $"Error al obtener información de conexión: {ex.Message}";
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Valida que la cadena de conexión sea válida
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando la cadena de conexión es inválida</exception>
        private void ValidateConnectionString()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                    throw new ArgumentException("La cadena de conexión no puede estar vacía");

                // Intentar crear un SqlConnectionStringBuilder para validar el formato
                var builder = new SqlConnectionStringBuilder(_connectionString);
                
                if (string.IsNullOrWhiteSpace(builder.DataSource))
                    throw new ArgumentException("El servidor no está especificado en la cadena de conexión");
                
                if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
                    throw new ArgumentException("La base de datos no está especificada en la cadena de conexión");
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Cadena de conexión inválida: {ex.Message}", ex);
            }
        }

        #endregion

        #region Static Helper Methods

        /// <summary>
        /// Método estático para obtener una conexión directamente (conveniencia)
        /// </summary>
        /// <returns>Nueva conexión SQL</returns>
        public static SqlConnection GetConnection()
        {
            return Instance.CreateConnection();
        }

        /// <summary>
        /// Método estático para probar la conexión
        /// </summary>
        /// <returns>True si la conexión funciona</returns>
        public static bool IsConnectionWorking()
        {
            return Instance.TestConnection();
        }

        #endregion
    }
}