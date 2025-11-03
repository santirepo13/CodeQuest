using System;

namespace CodeQuest.Validators
{
    /// <summary>
    /// Clase base abstracta para validadores - Implementa Abstracción y Herencia
    /// </summary>
    public abstract class BaseValidator
    {
        /// <summary>
        /// Nombre del validador
        /// </summary>
        public string ValidatorName { get; protected set; }

        /// <summary>
        /// Constructor protegido para clases derivadas
        /// </summary>
        /// <param name="validatorName">Nombre del validador</param>
        protected BaseValidator(string validatorName)
        {
            ValidatorName = validatorName ?? throw new ArgumentNullException(nameof(validatorName));
        }

        /// <summary>
        /// Método abstracto que debe ser implementado por las clases derivadas - Polimorfismo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>True si es válido, False en caso contrario</returns>
        public abstract bool IsValid(object value);

        /// <summary>
        /// Método virtual que puede ser sobrescrito - Polimorfismo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>Mensaje de error si no es válido</returns>
        public virtual string GetErrorMessage(object value)
        {
            return $"El valor '{value}' no es válido para {ValidatorName}";
        }

        /// <summary>
        /// Método que valida y lanza excepción si no es válido
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <exception cref="ArgumentException">Se lanza cuando el valor no es válido</exception>
        public void ValidateAndThrow(object value)
        {
            try
            {
                if (!IsValid(value))
                {
                    throw new ArgumentException(GetErrorMessage(value));
                }
            }
            catch (ArgumentException)
            {
                throw; // Re-lanza excepciones de validación
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error durante la validación: {ex.Message}", ex);
            }
        }
    }
}