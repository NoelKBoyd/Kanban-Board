using Kanban_Board.Classes;
using Kanban_Board.Services;
using System.Reflection;
using System.Text.Json;

namespace Kanban_Board
{
    class Program
    {
        private const string UserFilePath = "Users.json";
        private static List<User> users = new List<User>();

        static async Task Main(string[] args)
        {
            if (args.Contains("--clear"))
            {
                PrintColored($"'--clear' argument detected. Cleaning up system...", ConsoleColor.Yellow);
                try
                {
                    if (File.Exists(UserFilePath))
                    {
                        File.Delete(UserFilePath);
                        PrintColored("Users file removed successfully.", ConsoleColor.Green);
                    }
                    else
                    {
                        PrintColored("Users file not found.", ConsoleColor.Cyan);
                    }

                    string[] taskFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_tasks.bin");

                    if (taskFiles.Length > 0)
                    {
                        foreach (string file in taskFiles)
                        {
                            File.Delete(file);
                            PrintColored($"Deleted task file: {Path.GetFileName(file)}", ConsoleColor.Green);
                        }
                    }
                    else
                    {
                        PrintColored("No task files found to clean.", ConsoleColor.Cyan);
                    }

                    PrintColored("System reset complete. Starting fresh.", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    PrintColored($"Warning: Error during cleanup: {ex.Message}", ConsoleColor.Yellow);
                }
            }

            try
            {
                LoadUsers();
            }
            catch (Exception ex)
            {
                PrintColored($"Critical error loading users: {ex.Message}", ConsoleColor.Red);
                return;
            }

            User loggedInUser = null;

            Console.WriteLine("Welcome to the Kanban Board Authentication System!");

            // --- Main Menu ---

            while (loggedInUser == null)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("> ");

                string choice = Console.ReadLine();

                switch (choice?.Trim())
                {
                    case "1":
                        Register();
                        Console.Clear();
                        break;
                    case "2":
                        loggedInUser = await Login();
                        if (loggedInUser != null)
                        {
                            Console.Clear();
                            PrintColored($"\nLogin successful! Welcome {loggedInUser.Username}", ConsoleColor.Green);
                            Thread.Sleep(1000);
                        }
                        break;
                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

            TaskManager taskManager = new TaskManager();

            if (loggedInUser != null)
            {
                taskManager.LoadTasks(loggedInUser);
            }

            bool exit = false;
            while (!exit)
            {
                GUI.MainMenu.DisplayMenu();

                if (!int.TryParse(Console.ReadLine(), out int response))
                {
                    Console.Clear();
                    continue;
                }

                Console.Clear();
                switch (response)
                {
                    case 1:
                        GUI.MainMenu.DisplayBoards();
                        break;
                    case 2:
                        GUI.MainMenu.DisplayLists();
                        break;
                    case 3:
                        GUI.MainMenu.DisplayTasks(taskManager);
                        if (loggedInUser != null) taskManager.SaveTasks(loggedInUser); //auto save after exiting
                        break;
                    case 4:
                        exit = true;
                        if (loggedInUser != null)
                        {
                            taskManager.SaveTasks(loggedInUser);
                        }
                        Console.WriteLine("Thanks for using Kanban-Board!");
                        Thread.Sleep(1000);
                        break;
                    default:
                        GUI.MainMenu.DisplayMenu();
                        break;
                }
            }
        }

        // --- Register & Login ---

        private static void Register()
        {
            try
            {
                Console.WriteLine("\n--- Register New User ---");
                string username = PromptNonEmpty("Enter username: ");
                string charsToFind = "!@$%";

                if (FindUserByUsername(username) != null)
                {
                    PrintColored("Username already exists. Please try a different one.", ConsoleColor.Yellow);
                    Thread.Sleep(3000);
                    return;
                }

                Console.WriteLine("Enter password");
                Console.WriteLine("The parameters for a valid password are:");
                Console.WriteLine("- At least 6 characters long");
                Console.WriteLine("- Cannot be the same as the username");
                Console.WriteLine($"- Must contain at least one of these symbols: {charsToFind}");

                string password = ReadPassword();
                string errorMessage;

                bool isValid = User.ValidatePassword(password, username, out errorMessage);

                if (!isValid)
                {
                    throw new ArgumentException(errorMessage);
                }

                string hashedPassword;
                try
                {
                    hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                }
                catch (Exception ex)
                {
                    PrintColored($"Password hashing failed: {ex.Message}", ConsoleColor.Red);
                    Thread.Sleep(3000);
                    return;
                }

                var newUser = new User(username, hashedPassword);
                users.Add(newUser);

                try
                {
                    SaveUsers();
                }
                catch (Exception ex)
                {
                    PrintColored($"Could not save new user: {ex.Message}", ConsoleColor.Red);
                    users.Remove(newUser);
                    Thread.Sleep(3000);
                    return;
                }

                PrintColored($"\nUser '{username}' registered successfully!", ConsoleColor.Green);
                Console.WriteLine("Returning to menu...");
                Thread.Sleep(2000);
            }
            catch (ArgumentException ex)
            {
                PrintColored($"Registration failed: {ex.Message}", ConsoleColor.Red);
                Console.WriteLine("Returning to menu in 3 seconds...");
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                PrintColored($"An unexpected error occurred during registration: {ex.Message}", ConsoleColor.Red);
                Console.WriteLine("Returning to menu in 3 seconds...");
                Thread.Sleep(3000);
            }
        }

        private static async Task<User> Login()
        {
            try
            {
                Console.WriteLine("\n--- User Login ---");
                string username = PromptNonEmpty("Enter username: ");
                string password = PromptNonEmpty("Enter password: ", maskInput: true);

                var user = FindUserByUsername(username);
                if (user == null)
                {
                    Console.WriteLine("Login failed. User not found.");
                    return null;
                }

                bool isPasswordValid = false;
                try
                {
                    // Verifying hash
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Password verification failed: {ex.Message}");
                    return null;
                }

                if (isPasswordValid)
                {
                    return user;
                }
                else
                {
                    PrintColored("\nLogin failed. Invalid password.", ConsoleColor.Red);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return null;
            }
        }

        // --- Helpers ---

        private static User FindUserByUsername(string username)
        {
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        private static void LoadUsers()
        {
            try
            {
                if (File.Exists(UserFilePath))
                {
                    string json = File.ReadAllText(UserFilePath);
                    try
                    {
                        users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"User data file corrupted: {ex.Message}");
                        users = new List<User>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                users = new List<User>();
            }
        }

        private static void SaveUsers()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(UserFilePath, json);
        }

        public static string ReadPassword()
        {
            string password = "";
            try
            {
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password[0..^1];
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                }
                while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading password: {ex.Message}");
                return "";
            }
            return password;
        }

        private static void PrintColored(string message, ConsoleColor color)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prevColor;
        }

        private static string PromptNonEmpty(string prompt, bool maskInput = false)
        {
            while (true)
            {
                Console.Write(prompt);
                string input;
                if (maskInput)
                {
                    input = ReadPassword();
                }
                else
                {
                    input = Console.ReadLine();
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                }
                else
                {
                    return input;
                }
            }
        }
    }
}