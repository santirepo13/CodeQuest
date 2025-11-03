using Microsoft.Data.SqlClient;

namespace CodeQuest.Factories
{
    public interface IDatabaseConnectionFactory
    {
        SqlConnection CreateConnection();
    }

    public class SqlServerConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string connectionString;

        public SqlServerConnectionFactory()
        {
            connectionString = @"Server=DESKTOP-FN66L1D\SQLEXPRESS;Database=CodeQuest;Integrated Security=true;TrustServerCertificate=true;";
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}