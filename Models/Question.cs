using System;
using System.Collections.Generic;

namespace CodeQuest.Models
{
    /// <summary>
    /// Clase que representa una pregunta de opción múltiple del juego
    /// Implementa Encapsulamiento con validaciones
    /// </summary>
    public class Question
    {
        private string _text;
        private int _difficulty;

        /// <summary>
        /// ID único de la pregunta
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        /// Texto de la pregunta con validaciones de encapsulamiento
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando el texto es inválido</exception>
        public string Text 
        { 
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El texto de la pregunta no puede estar vacío");
                if (value.Length > 1000)
                    throw new ArgumentException("El texto de la pregunta no puede exceder 1000 caracteres");
                
                _text = value.Trim();
            }
        }

        /// <summary>
        /// Dificultad de la pregunta (1-3) con validación
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando la dificultad está fuera del rango</exception>
        public int Difficulty 
        { 
            get => _difficulty;
            set
            {
                if (value < 1 || value > 3)
                    throw new ArgumentException("La dificultad debe estar entre 1 y 3");
                _difficulty = value;
            }
        }

        /// <summary>
        /// Lista de opciones de respuesta
        /// </summary>
        public List<Choice> Choices { get; set; } = new List<Choice>();

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Question()
        {
            _text = string.Empty;
            _difficulty = 1;
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="text">Texto de la pregunta</param>
        /// <param name="difficulty">Dificultad (1-3)</param>
        public Question(string text, int difficulty)
        {
            Text = text; // Usa la propiedad para validar
            Difficulty = difficulty; // Usa la propiedad para validar
        }

        /// <summary>
        /// Calcula los puntos base según la dificultad
        /// </summary>
        /// <returns>Puntos base que otorga esta pregunta</returns>
        public int GetBasePoints()
        {
            return Difficulty * 10; // Más puntos por mayor dificultad
        }

        /// <summary>
        /// Obtiene el tipo de pregunta
        /// </summary>
        /// <returns>Descripción del tipo de pregunta</returns>
        public string GetQuestionType()
        {
            return $"Pregunta de Opción Múltiple - Dificultad {Difficulty}";
        }

        /// <summary>
        /// Valida que la pregunta tenga al menos una respuesta correcta
        /// </summary>
        /// <returns>True si la pregunta es válida</returns>
        public bool IsValid()
        {
            try
            {
                if (Choices == null || Choices.Count == 0)
                    return false;

                foreach (var choice in Choices)
                {
                    if (choice.IsCorrect)
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Clase que representa una opción de respuesta
    /// Implementa Encapsulamiento con validaciones
    /// </summary>
    public class Choice
    {
        private string _choiceText;

        /// <summary>
        /// ID único de la opción
        /// </summary>
        public int ChoiceID { get; set; }

        /// <summary>
        /// Texto de la opción con validaciones
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando el texto es inválido</exception>
        public string ChoiceText 
        { 
            get => _choiceText;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El texto de la opción no puede estar vacío");
                if (value.Length > 500)
                    throw new ArgumentException("El texto de la opción no puede exceder 500 caracteres");
                
                _choiceText = value.Trim();
            }
        }

        /// <summary>
        /// Indica si esta opción es la respuesta correcta
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Choice()
        {
            _choiceText = string.Empty;
            IsCorrect = false;
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="choiceText">Texto de la opción</param>
        /// <param name="isCorrect">Si es la respuesta correcta</param>
        public Choice(string choiceText, bool isCorrect = false)
        {
            ChoiceText = choiceText; // Usa la propiedad para validar
            IsCorrect = isCorrect;
        }
    }
}