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
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new FormStart());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la aplicación: {ex.Message}\n\n" +
                    $"Detalles: {ex.GetType().Name}\n" +
                    $"Si es un error de base de datos, verifique que SQL Server esté ejecutándose y que la base de datos 'CodeQuest' exista.",
                    "Error de Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
