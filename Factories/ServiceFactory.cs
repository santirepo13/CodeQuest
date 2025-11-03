using CodeQuest.Repositories;
using CodeQuest.Services;

namespace CodeQuest.Factories
{
    public static class ServiceFactory
    {
        private static IDatabaseConnectionFactory connectionFactory;
        private static IUserRepository userRepository;
        private static IQuestionRepository questionRepository;
        private static IRoundRepository roundRepository;
        private static IGameService gameService;

        public static IGameService GetGameService()
        {
            if (gameService == null)
            {
                connectionFactory = new SqlServerConnectionFactory();
                userRepository = new UserRepository(connectionFactory);
                questionRepository = new QuestionRepository(connectionFactory);
                roundRepository = new RoundRepository(connectionFactory);
                gameService = new GameService(userRepository, questionRepository, roundRepository);
            }
            
            return gameService;
        }
    }
}