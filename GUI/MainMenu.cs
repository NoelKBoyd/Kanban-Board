using Kanban_Board.Classes;
using Kanban_Board.Enums;
using Kanban_Board.Services;
using System;
using static Kanban_Board.GUI.TaskMenu;
using static Kanban_Board.GUI.ListMenu;
using static Kanban_Board.GUI.BoardMenu;

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
                            ViewTasks(taskManager);
                            break;

                        case 2:
                            CreateTask(taskManager);
                            break;

                        case 3:
                            EditTask(taskManager);
                            break;

                        case 4:
                            DeleteTask(taskManager);
                            break;

                        case 5:
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ReadKey();
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

        public static void DisplayLists()
        {
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
                            // View Lists
                            break;
                        case 2:
                            // Create Lists
                            break;
                        case 3:
                            // Edit Lists
                            break;
                        case 4:
                            // Delete Lists
                            break;
                        case 5:
                            // Add Tasks to Lists
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

        public static void DisplayBoards()
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

                string? input = Console.ReadLine();
                if (int.TryParse(input, out int response))
                {
                    Console.Clear();
                    switch (response)
                    {
                        case 1:
                            // View Boards
                            break;
                        case 2:
                            // Create Boards
                            break;
                        case 3:
                            // Edit Boards
                            break;
                        case 4:
                            // Delete Boards
                            break;
                        case 5:
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

        

        public static DateTime GetDeadlineFromUser()
        {
            const string format = "DD-MM-YYYY";
            string deadlineInput;

            Console.WriteLine("1: Today");
            Console.WriteLine("2: In a Week");
            Console.WriteLine("3: In a Fortnight");
            Console.WriteLine("4: In a Month");
            Console.WriteLine("5: Custom Date");

            string? input = Console.ReadLine();
            if (int.TryParse(input, out int response))
            {
                switch (response)
                {
                    case 1:
                        deadlineInput = DateTime.Now.ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 2:
                        deadlineInput = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 3:
                        deadlineInput = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 4:
                        deadlineInput = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 5:
                        Console.WriteLine($"Please enter a deadline ({format}):");
                        deadlineInput = Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
            return DateTime.Now;
        }

        public static Priority GetPriorityFromUser()
        {
            Priority Priority;
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. High");
            string? userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int Out))
            {
                switch (Out)
                {
                    case 1:
                        Priority = Priority.Low;
                        break;
                    case 2:
                        Priority = Priority.Medium;
                        break;
                    case 3:
                        Priority = Priority.High;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Defaulting to Low priority.");
                        Priority = Priority.Low;
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Defaulting to Low priority.");
                Priority = Priority.Low;
            }
            return Priority;
        }

        public static Status GetStatusFromUser()
        {
            Status Status;
            Console.WriteLine("1. To Do");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Done");
            string? userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int Out))
            {
                switch (Out)
                {
                    case 1:
                        Status = Status.ToDo;
                        break;
                    case 2:
                        Status = Status.InProgress;
                        break;
                    case 3:
                        Status = Status.Done;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Defaulting to To Do status.");
                        Status = Status.ToDo;
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Defaulting to To Do status.");
                Status = Status.ToDo;
            }
            return Status;
        }
    }
}
