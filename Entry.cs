namespace Kanban_Board
{
    internal class Entry
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                int response = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (response)
                {
                    case 1:
                        DisplayBoards();
                        break;
                    case 2:
                        DisplayLists();
                        break;
                    case 3:
                        DisplayTasks();
                        break;
                    case 4:
                        exit = true;
                        Console.WriteLine("Thanks for using kanban-Board!");
                        Thread.Sleep(2000);
                        break;
                    default:
                        DisplayMenu();
                        break;
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("------------------Kanban--ToDo------------------");
            Console.WriteLine("Please select from one of the following options:");
            Console.WriteLine("1. Boards");
            Console.WriteLine("2. Lists");
            Console.WriteLine("3. Tasks");
            Console.WriteLine("4. Exit");
            Console.WriteLine("------------------------------------------------");
        }

        static void DisplayBoards()
        {
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
                Console.ReadLine();

                if (int.TryParse(Console.ReadLine(), out int response))
                {
                    switch (response)
                    {
                        case 1:
                            //View Boards
                            break;
                        case 2:
                            //Create Boards
                            break;
                        case 3:
                            //Edit Boards
                            break;
                        case 4:
                            //Delete Boards
                            break;
                        case 5:
                            //Add Lists to Boards
                            break;
                        case 6:
                            //Exit to Main Menu
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }

        static void DisplayLists()
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
            Console.ReadLine();
        }

        static void DisplayTasks()
        {
            Console.WriteLine("----------------------Tasks---------------------");
            Console.WriteLine("Please select from one of the following options:");
            Console.WriteLine("1. View Tasks");
            Console.WriteLine("2. Create Tasks");
            Console.WriteLine("3. Edit Tasks");
            Console.WriteLine("4. Delete Tasks");
            Console.WriteLine("5. Add Tasks to Lists");
            Console.WriteLine("6. Exit to Main Menu");
            Console.WriteLine("------------------------------------------------");
            Console.ReadLine();
        }
    }
}
