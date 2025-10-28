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
                if (int.TryParse(Console.ReadLine(), out int response))
                {
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
                            GUI.Menu.DisplayTasks(taskManager);
                            break;
                        case 4:
                            exit = true;
                            Console.WriteLine("Thanks for using kanban-Board!");
                            Thread.Sleep(1000);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please choose a number from 1 to 4.");
                            Thread.Sleep(1000);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Thread.Sleep(1000);
                }
                if (!exit) Console.Clear();
            }
        }
    }
}