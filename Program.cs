using System;
using System.Windows.Forms;
using CodeQuest.Factories;

namespace CodeQuest
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // Verificar que el servicio funcione con el Singleton
                var gameService = ServiceFactory.GetGameService();
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormInicio());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la aplicación: {ex.Message}", "Error de Inicialización", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}