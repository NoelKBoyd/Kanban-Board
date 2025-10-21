using Kanban_Board.Services;

namespace Kanban_Board
{
    internal class Entry
    {
        public static void Main(string[] args)
        {
            // Create an instance of TaskManager to pass to DisplayTasks
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
