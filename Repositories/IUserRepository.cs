using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    public interface IUserRepository
    {
        int CreateUser(string username);
        bool UserExists(string username);
        int GetUserId(string username);
        User GetUserById(int userId);
    }
}