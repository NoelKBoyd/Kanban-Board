using Kanban_Board.Services;

namespace Kanban_Board.GUI
{
    internal class MainMenu
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("------------------Kanban--ToDo------------------");
            Console.WriteLine("Please select from one of the following options:");
            Console.WriteLine("1. Boards");
            Console.WriteLine("2. Lists");
            Console.WriteLine("3. Tasks");
            Console.WriteLine("4. Exit");
            Console.WriteLine("------------------------------------------------");
        }

        public static void DisplayTasks(TaskManager taskManager)
        {
            Console.Clear();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----------------------Tasks---------------------");
                Console.WriteLine("Please select from one of the following options:");
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Create Tasks");
                Console.WriteLine("3. Edit Tasks");
                Console.WriteLine("4. Delete Tasks");
                Console.WriteLine("5. Exit to Main Menu");
                Console.WriteLine("------------------------------------------------");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int response))
                {
                    Console.Clear();
                    switch (response)
                    {
                        case 1:
                            TaskMenu.ViewTasks(taskManager);
                            break;

                        case 2:
                            TaskMenu.CreateTask(taskManager);
                            break;

                        case 3:
                            TaskMenu.EditTask(taskManager);
                            break;

                        case 4:
                            TaskMenu.DeleteTask(taskManager);
                            break;

                        case 5:
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.ReadKey();
                }
            }
        }

        public static void DisplayLists(ListManager listManager)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("----------------------Lists---------------------");
                Console.WriteLine("Please select from one of the following options:");
                Console.WriteLine("1. View Lists");
                Console.WriteLine("2. Create Lists");
                Console.WriteLine("3. Edit Lists");
                Console.WriteLine("4. Delete Lists");
                Console.WriteLine("5. Add Tasks to Lists");
                Console.WriteLine("6. Exit to Main Menu");
                Console.WriteLine("------------------------------------------------");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int response))
                {
                    Console.Clear();
                    switch (response)
                    {
                        case 1:
                            ListMenu.ViewLists(listManager);
                            break;
                        case 2:
                            ListMenu.CreateList(listManager);
                            break;
                        case 3:
                            ListMenu.EditList(listManager);
                            break;
                        case 4:
                            ListMenu.DeleteList(listManager);
                            break;
                        case 5:
                            break;
                        case 6: // Exit to Main Menu
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        public static void DisplayBoards(BoardManager boardManager)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("---------------------Boards---------------------");
                Console.WriteLine("Please select from one of the following options:");
                Console.WriteLine("1. View Boards");
                Console.WriteLine("2. Create Boards");
                Console.WriteLine("3. Edit Boards");
                Console.WriteLine("4. Delete Boards");
                Console.WriteLine("5. Add Lists to Boards");
                Console.WriteLine("6. Exit to Main Menu");
                Console.WriteLine("------------------------------------------------");

                string? input = Console.ReadLine();
                if (int.TryParse(input, out int response))
                {
                    Console.Clear();
                    switch (response)
                    {
                        case 1:
                            Console.Clear();
                            BoardMenu.ViewBoards(boardManager);
                            break;
                        case 2:
                            Console.Clear();
                            BoardMenu.CreateBoard(boardManager);
                            break;
                        case 3:
                            Console.Clear();
                            BoardMenu.EditBoard(boardManager);
                            break;
                        case 4:
                            Console.Clear();
                            BoardMenu.DeleteBoard(boardManager);
                            break;
                        case 5:
                            Console.Clear();
                            // Add Lists to Boards
                            break;
                        case 6: // Exit to Main Menu
                            return;
                        default: //handle the input being a numeric but out of range
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }
    }
}
