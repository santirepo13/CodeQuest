using CodeQuest.Services;
using CodeQuest.Database;

namespace CodeQuest.Factories
{
    /// <summary>
    /// Factory para servicios que usa el patrón Singleton para conexiones
    /// </summary>
    public static class ServiceFactory
    {
        private static IGameService gameService;
        private static readonly object _lock = new object();

        /// <summary>
        /// Obtiene la instancia del servicio de juego (thread-safe)
        /// Usa el patrón Singleton DbConnection internamente
        /// </summary>
        /// <returns>Instancia del servicio de juego</returns>
        public static IGameService GetGameService()
        {
            if (gameService == null)
            {
                lock (_lock)
                {
                    if (gameService == null)
                    {
                        // Verificar que la conexión funcione antes de crear el servicio
                        if (!DbConnection.IsConnectionWorking())
                        {
                            throw new System.InvalidOperationException("No se puede conectar a la base de datos");
                        }
                        
                        gameService = new GameService();
                    }
                }
            }
            
            return gameService;
        }


    }
}