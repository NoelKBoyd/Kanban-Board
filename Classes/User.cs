namespace Kanban_Board.Classes
{
    internal class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public bool isPublic { get; set; }

        public User() { }

        public User(string username, string hashedPassword)
        {
            Username = username;
            HashedPassword = hashedPassword;
        }
    }
}
