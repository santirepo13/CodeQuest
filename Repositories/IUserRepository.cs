using System.Collections.Generic;
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

        /// <summary>
        /// Actualiza la información de un usuario
        /// </summary>
        /// <param name="user">Usuario con la información actualizada</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateUser(User user);

        /// <summary>
        /// Elimina un usuario de la base de datos
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteUser(int userId);

        /// <summary>
        /// Obtiene todos los usuarios (para administración)
        /// </summary>
        /// <returns>Lista de todos los usuarios</returns>
        List<User> GetAllUsers();

        // Métodos administrativos con procedimientos almacenados
        /// <summary>
        /// Actualiza solo el nombre de usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="newUsername">Nuevo nombre de usuario</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateUsername(int userId, string newUsername);

        /// <summary>
        /// Elimina completamente un usuario y todos sus datos
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteUserComplete(int userId);

        /// <summary>
        /// Resetea el XP de un usuario a 0
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se reseteó correctamente</returns>
        bool ResetUserXP(int userId);
    }
}