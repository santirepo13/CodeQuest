using System;

namespace CodeQuest.Models
{
    /// <summary>
    /// Clase que representa un usuario del juego
    /// Implementa el pilar de Encapsulamiento de POO con propiedades privadas y validaciones
    /// </summary>
    public class User
    {
        private string _username;
        private int _xp;
        private int _level;

        /// <summary>
        /// ID único del usuario
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Nombre de usuario con validaciones de encapsulamiento
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando el username es inválido</exception>
        public string Username 
        { 
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío");
                if (value.Length < 3)
                    throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres");
                if (value.Length > 50)
                    throw new ArgumentException("El nombre de usuario no puede exceder 50 caracteres");
                
                _username = value.Trim();
            }
        }

        /// <summary>
        /// Puntos de experiencia del usuario con validación
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando el XP es negativo</exception>
        public int Xp 
        { 
            get => _xp;
            set
            {
                if (value < 0)
                    throw new ArgumentException("El XP no puede ser negativo");
                _xp = value;
            }
        }

        /// <summary>
        /// Nivel del usuario calculado automáticamente basado en XP
        /// </summary>
        public int Level 
        { 
            get => _level;
            set
            {
                if (value < 1)
                    throw new ArgumentException("El nivel debe ser mayor a 0");
                _level = value;
            }
        }

        /// <summary>
        /// Fecha de creación del usuario
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public User()
        {
            _username = string.Empty;
            _xp = 0;
            _level = 1;
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="xp">Puntos de experiencia iniciales</param>
        public User(string username, int xp = 0)
        {
            Username = username; // Usa la propiedad para validar
            Xp = xp; // Usa la propiedad para validar
            Level = 1 + (xp / 100); // Calcula el nivel basado en XP
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Calcula el XP necesario para el siguiente nivel
        /// </summary>
        /// <returns>XP faltante para subir de nivel</returns>
        public int GetXpForNextLevel()
        {
            return (Level * 100) - Xp;
        }

        /// <summary>
        /// Añade XP al usuario y recalcula el nivel
        /// </summary>
        /// <param name="xpToAdd">XP a añadir</param>
        /// <exception cref="ArgumentException">Se lanza cuando el XP a añadir es negativo</exception>
        public void AddXp(int xpToAdd)
        {
            if (xpToAdd < 0)
                throw new ArgumentException("No se puede añadir XP negativo");
            
            Xp += xpToAdd;
            Level = 1 + (Xp / 100); // Recalcula el nivel
        }
    }
}