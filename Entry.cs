using Kanban_Board.Services;
using System.Reflection;

namespace Kanban_Board
{
    internal class Entry
    {
        public static void Main(string[] args)
        {
            if (args.Contains("-v") || args.Contains("--version"))
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                Console.WriteLine($"Kanban-Board Version: {version}");
            }

            TaskManager taskManager = new TaskManager();

            bool exit = false;
            while (!exit)
            {
                GUI.Menu.DisplayMenu();
                int response = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (response)
                {
                    case 1:
                        GUI.Menu.DisplayBoards();
                        break;
                    case 2:
                        GUI.Menu.DisplayLists();
                        break;
                    case 3:
                        GUI.Menu.DisplayTasks(taskManager); // Pass taskManager as argument
                        break;
                    case 4:
                        exit = true;
                        Console.WriteLine("Thanks for using kanban-Board!");
                        Thread.Sleep(1000);
                        break;
                    default:
                        GUI.Menu.DisplayMenu();
                        break;
                }
            }
        }
    }
}