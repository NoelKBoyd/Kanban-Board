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

        public static bool ValidatePassword(string password, string username, out string errorMessage)
        {
            string specialChars = "!@$%";

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                errorMessage = "Password must be at least 6 characters long.";
                return false;
            }

            if (password == username)
            {
                errorMessage = "Password cannot be the same as the username.";
                return false;
            }

            if (!password.Any(c => specialChars.Contains(c)))
            {
                errorMessage = $"Password must contain at least one of these symbols: {specialChars}";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
