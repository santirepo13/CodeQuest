using System;

namespace CodeQuest.Validators
{
    /// <summary>
    /// Validador específico para el juego que hereda de BaseValidator
    /// Demuestra Herencia y Polimorfismo de manera simple
    /// </summary>
    public class GameValidator : BaseValidator
    {
        /// <summary>
        /// Constructor que llama al constructor base
        /// </summary>
        public GameValidator() : base("Game Validator")
        {
        }

        /// <summary>
        /// Implementación polimórfica para validar datos del juego
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>True si es válido</returns>
        public override bool IsValid(object value)
        {
            try
            {
                if (value == null) return false;
                
                // Validación simple: no puede ser string vacío o número negativo
                if (value is string stringValue)
                {
                    return !string.IsNullOrWhiteSpace(stringValue);
                }
                
                if (value is int intValue)
                {
                    return intValue >= 0;
                }
                
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
                    return "El valor no puede ser nulo";
                
                if (value is string && string.IsNullOrWhiteSpace(value.ToString()))
                    return "El texto no puede estar vacío";
                
                if (value is int intValue && intValue < 0)
                    return "Los números no pueden ser negativos";
                
                return base.GetErrorMessage(value);
            }
            catch (Exception ex)
            {
                return $"Error en validación: {ex.Message}";
            }
        }
    }
}