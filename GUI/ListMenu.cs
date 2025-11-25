using Kanban_Board.Classes;
using Kanban_Board.Enums;
using Kanban_Board.Services;
using System.Diagnostics;
using static Kanban_Board.GUI.InputHelper;

namespace Kanban_Board.GUI
{
    internal class ListMenu
    {
        public static void ViewLists(ListManager listManager, bool pause = true)
        {
            Console.Clear();
            List<KanbanList> listList = listManager.GetLists();
            Console.WriteLine("--- All Lists ---");
            if (listList.Count == 0)
            {
                Console.WriteLine("No lists to display. Please create a list first.");
                if (!pause)
                {
                    Console.WriteLine("Press any key to return.");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }
            else
            {
                for (int i = 0; i < listList.Count; i++)
                {
                    KanbanList list = listList[i];

                    Console.WriteLine($"ID: {list.Id}");
                    Console.WriteLine($"Title: {list.Title}");
                    Console.WriteLine($"Description: {list.Description}");
                    Console.WriteLine($"Status: {list.Status}");
                    Console.WriteLine("-----------------");
                }
            }

            if (pause)
            {
                Console.WriteLine("Press any key to return to the menu.");
                Console.ReadKey();
            }
        }

        public static void CreateList(ListManager listManager)
        {
            Console.Clear();
            string title = "";
            while (string.IsNullOrWhiteSpace(title))
            {
                Console.Clear();
                Console.WriteLine("--- Title ---");

                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    title = input;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Title cannot be empty.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
            }
            Console.Clear();
            Console.WriteLine("--- Description ---");
            string description = Console.ReadLine() ?? "";
            Console.Clear();
            listManager.CreateList(title, description);
            Console.WriteLine("List created!");
            Thread.Sleep(1000);
        }

        public static void EditList(ListManager listManager)
        {
            Console.Clear();
            ViewLists(listManager, pause: false);

            Console.WriteLine("\nEnter the ID of the list you want to edit:");
            Console.WriteLine("Or press Enter to cancel.");
            if (int.TryParse(Console.ReadLine(), out int inputId))
            {
                KanbanList listToEdit = listManager.GetListById(inputId);

                if (listToEdit != null)
                {
                    Console.WriteLine($"Editing List: {listToEdit.Title}");
                    Console.WriteLine("Which field would you like to update?");
                    Console.WriteLine("1. Title");
                    Console.WriteLine("2. Description");
                    Console.WriteLine("3. Cancel");

                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("Enter new Title:");
                                string newTitle = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newTitle))
                                    listToEdit.Title = newTitle;
                                break;
                            case 2:
                                Console.WriteLine("Enter new Description:");
                                listToEdit.Description = Console.ReadLine();
                                break;
                            default:
                                Console.WriteLine("Edit cancelled.");
                                return;
                        }
                        Console.WriteLine("List updated successfully!");
                        Console.WriteLine("Press enter to return to the menu");
                    }
                    else
                    {
                        Console.WriteLine("List not found with that ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID format.");
                }
            }
        }

        public static void DeleteList(ListManager listManager)
        {
            Console.Clear();
            ViewLists(listManager, pause: false);
            Console.WriteLine("\nEnter the ID of the list you want to delete:");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int response))
            {
                KanbanList listToDelete = listManager.GetListById(response);
                if (listToDelete != null)
                {
                    listManager.DeleteList(listToDelete);
                    Console.WriteLine("List deleted successfully!");
                    Console.WriteLine("Press any key to return to the menu.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("List not found with that ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }
    }
}
