using Kanban_Board.Classes;

namespace Kanban_Board.Services
{
    internal class UserManager
    {
        public void CreateUser(string username, string hashedPassword, bool isPublic)
        {
            User newUser = new User
            {
                Username = username,
                HashedPassword = hashedPassword,
                isPublic = isPublic
            };
        }
    }
}
