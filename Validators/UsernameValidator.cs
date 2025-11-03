using System;
using System.Text.RegularExpressions;

namespace CodeQuest.Validators
{
    /// <summary>
    /// Validador específico para nombres de usuario - Implementa Herencia y Polimorfismo
    /// </summary>
    public class UsernameValidator : BaseValidator
    {
        private readonly int _minLength;
        private readonly int _maxLength;
        private readonly Regex _allowedCharsRegex;

        /// <summary>
        /// Constructor con parámetros de validación
        /// </summary>
        /// <param name="minLength">Longitud mínima del username</param>
        /// <param name="maxLength">Longitud máxima del username</param>
        public UsernameValidator(int minLength = 3, int maxLength = 50) : base("Username")
        {
            _minLength = minLength > 0 ? minLength : 3;
            _maxLength = maxLength > minLength ? maxLength : 50;
            _allowedCharsRegex = new Regex(@"^[a-zA-Z0-9_]+$");
        }

        /// <summary>
        /// Implementación polimórfica de validación de username
        /// </summary>
        /// <param name="value">Username a validar</param>
        /// <returns>True si es válido</returns>
        public override bool IsValid(object value)
        {
            try
            {
                if (value == null) return false;
                
                string username = value.ToString().Trim();
                
                // Validar longitud
                if (username.Length < _minLength || username.Length > _maxLength)
                    return false;
                
                // Validar caracteres permitidos (solo letras, números y guión bajo)
                if (!_allowedCharsRegex.IsMatch(username))
                    return false;
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Implementación polimórfica del mensaje de error
        /// </summary>
        /// <param name="value">Valor que falló la validación</param>
        /// <returns>Mensaje de error específico</returns>
        public override string GetErrorMessage(object value)
        {
            try
            {
                if (value == null)
                    return "El nombre de usuario no puede ser nulo";
                
                string username = value.ToString().Trim();
                
                if (username.Length < _minLength)
                    return $"El nombre de usuario debe tener al menos {_minLength} caracteres";
                
                if (username.Length > _maxLength)
                    return $"El nombre de usuario no puede exceder {_maxLength} caracteres";
                
                if (!_allowedCharsRegex.IsMatch(username))
                    return "El nombre de usuario solo puede contener letras, números y guión bajo";
                
                return base.GetErrorMessage(value);
            }
            catch (Exception ex)
            {
                return $"Error al generar mensaje de validación: {ex.Message}";
            }
        }
    }
}