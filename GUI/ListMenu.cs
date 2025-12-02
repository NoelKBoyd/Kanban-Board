using Kanban_Board.Classes;
using Kanban_Board.Services;
using System.Diagnostics;

namespace Kanban_Board.GUI
{
    internal class ListMenu
    {
        public static void ViewLists(ListManager listManager, TaskManager taskManager, bool pause = true)
        {
            Console.Clear();
            Dictionary<int, KanbanList> listList = listManager.GetLists();

            Console.WriteLine("--- Kanban Board Lists ---");

            if (listList.Count == 0)
            {
                Console.WriteLine("No lists to display. Please create a list first.");
                if (pause)
                {
                    Console.WriteLine("\nPress any key to return.");
                    Console.ReadKey();
                    Console.Clear();
                }
                return;
            }
            else
            {
                foreach (var list in listList.Values)
                {

                    Console.WriteLine($"ID: {list.Id}");
                    Console.WriteLine($"Title: {list.Title}");
                    Console.WriteLine($"Description: {list.Description}");
                    Console.WriteLine($"Status: {list.Status}");
                    Console.WriteLine("-----------------");
                }
            }

            // if pause is false, this function is being used as a helper 
            if (!pause) return;

            while (true)
            {
                Console.WriteLine("\nOptions:");
                Console.WriteLine("- Enter a [List ID] to view its tasks");
                Console.WriteLine("- Press [Enter] to return to the menu");
                Console.Write("> ");

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                if (int.TryParse(input, out int listId))
                {
                    KanbanList? selectedList = listManager.GetListById(listId);

                    if (selectedList != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\n--- Tasks in '{selectedList.Title}' ---");
                        Console.ResetColor();

                        if (selectedList.TaskIds.Count == 0)
                        {
                            Console.WriteLine("(This list is empty)");
                        }
                        else
                        {
                            foreach (var taskId in selectedList.TaskIds)
                            {
                                KanbanTask? task = taskManager.GetTaskById(taskId);
                                if (task != null)
                                {
                                    Console.WriteLine($"ID: {task.Id}");
                                    Console.WriteLine($"Title: {task.Title}");
                                    Console.WriteLine($"Description: {task.Description}");
                                    Console.WriteLine($"Status: {task.Status}");
                                    Console.WriteLine($"Priority: {task.Priority}");
                                    Console.WriteLine($"Deadline: {task.Deadline.ToShortDateString()}");
                                    Console.WriteLine("-----------------");
                                }
                            }
                        }
                        Console.WriteLine("-----------------------");
                    }
                    else
                    {
                        Console.WriteLine($"List with ID {listId} not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format. Please enter a number.");
                }
            }

            Console.Clear();
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

        public static void EditList(ListManager listManager, TaskManager taskManager)
        {
            Console.Clear();
            ViewLists(listManager, taskManager, pause: false);

            Console.WriteLine("\nEnter the ID of the list you want to edit:");
            Console.WriteLine("Or press Enter to cancel.");
            Console.WriteLine("");
            if (int.TryParse(Console.ReadLine(), out int inputId))
            {
                KanbanList listToEdit = listManager.GetListById(inputId);

                if (listToEdit != null)
                {
                    Console.WriteLine("");
                    Console.WriteLine($"Editing List: {listToEdit.Title}");
                    Console.WriteLine("");
                    Console.WriteLine("Which field would you like to update?");
                    Console.WriteLine("1. Title");
                    Console.WriteLine("2. Description");
                    Console.WriteLine("3. Cancel");
                    Console.WriteLine("");

                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("");
                                Console.WriteLine("Enter new Title:");
                                string newTitle = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newTitle))
                                    listToEdit.Title = newTitle;
                                break;
                            case 2:
                                Console.WriteLine("");
                                Console.WriteLine("Enter new Description:");
                                listToEdit.Description = Console.ReadLine();
                                break;
                            default:
                                Console.WriteLine("");
                                Console.WriteLine("Edit cancelled.");
                                return;
                        }
                        Console.WriteLine("List updated successfully!");
                        Console.WriteLine("Press enter to return to the menu");
                        Console.Clear();
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

        public static void DeleteList(ListManager listManager, TaskManager taskManager)
        {
            Console.Clear();
            ViewLists(listManager, taskManager, pause: false);
            Console.WriteLine("\nEnter the ID of the list you want to delete:");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int response))
            {
                KanbanList listToDelete = listManager.GetListById(response);
                if (listToDelete != null)
                {
                    listManager.DeleteList(listToDelete);
                    Console.WriteLine("List deleted successfully!");
                    Thread.Sleep(1000);
                    Console.Clear();
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

        public static void MoveTasksToList(ListManager listManager, TaskManager taskManager)
        {
            Console.Clear();
            ViewLists(listManager, taskManager, pause: false);

            Console.WriteLine("\n--- Move/Add Tasks ---");
            Console.WriteLine("Enter the ID of the DESTINATION List:");

            if (!int.TryParse(Console.ReadLine(), out int targetListId))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var allTasks = taskManager.GetTasks();
            if (allTasks.Count == 0)
            {
                Console.WriteLine("There are no available tasks.");
                return;
            }

            Console.WriteLine("\nAvailable Tasks:");
            foreach (var task in allTasks.Values)
            {
                Console.WriteLine($"ID: {task.Id} | Title: {task.Title} | Description: {task.Description}");
            }

            Console.WriteLine("\nEnter the IDs of the tasks to move/add (comma separated):");
            string? taskInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(taskInput)) return;

            string[] taskIds = taskInput.Split(',');
            int successCount = 0;

            foreach (var idStr in taskIds)
            {
                if (int.TryParse(idStr.Trim(), out int taskId))
                {
                    KanbanTask? taskObj = taskManager.GetTaskById(taskId);

                    if (taskObj != null)
                    {
                        //pass task ID instead of the task object
                        bool success = listManager.AddOrMoveTask(taskId, targetListId);

                        if (success) successCount++;
                    }
                    else
                    {
                        Console.WriteLine($"Task ID {taskId} does not exist in the database.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }

            if (successCount > 0)
            {
                Console.WriteLine($"\nSuccessfully processed {successCount} task(s)!");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
