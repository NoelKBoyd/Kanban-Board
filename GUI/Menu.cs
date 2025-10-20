using Kanban_Board.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban_Board.GUI
{
    internal class Menu
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

        public static void DisplayTasks()
        {
            while (true)
            {
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
                            // View Tasks
                            break;
                        case 2:
                            // Create Tasks
                            break;
                        case 3:
                            // Edit Tasks
                            break;
                        case 4:
                            // Delete Tasks
                            break;
                        case 5: // Exit to Main Menu
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
                else //handle the parse if its empty or a string
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
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
    }
}
