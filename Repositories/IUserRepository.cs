using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para usuarios
    /// Implementa el pilar de Abstracción de POO
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Crea un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="username">Nombre de usuario único</param>
        /// <returns>ID del usuario creado</returns>
        /// <exception cref="System.Exception">Se lanza cuando no se puede crear el usuario</exception>
        int CreateUser(string username);

        /// <summary>
        /// Verifica si un usuario existe en la base de datos
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si el usuario existe, False en caso contrario</returns>
        bool UserExists(string username);

        /// <summary>
        /// Obtiene el ID de un usuario por su nombre
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>ID del usuario o 0 si no existe</returns>
        int GetUserId(string username);

        /// <summary>
        /// Obtiene un usuario completo por su ID
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Objeto User con toda la información o null si no existe</returns>
        User GetUserById(int userId);
    }
}