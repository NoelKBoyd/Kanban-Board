using Kanban_Board.Classes;

namespace Kanban_Board.Services
{
    internal class UserManager
    {
        public void CreateUser(string username, string password, bool isPublic)
        {
            User newUser = new User
            {
                Username = username,
                Password = password,
                isPublic = isPublic
            };
        }
    }
}
