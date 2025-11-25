using Kanban_Board.Classes;
using Kanban_Board.Enums;
using Kanban_Board.Services;
using static Kanban_Board.GUI.InputHelper;

namespace Kanban_Board.GUI
{
    internal class TaskMenu
    {
        public static void DeleteTask(TaskManager taskManager)
        {
            Console.Clear();
            ViewTasks(taskManager, pause: false);
            Console.WriteLine("\nEnter the ID of the task you want to delete:");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int response))
            {
                KanbanTask taskToDelete = taskManager.GetTaskById(response);
                if (taskToDelete != null)
                {
                    taskManager.DeleteTask(taskToDelete);
                    Console.WriteLine("Task deleted successfully!");
                    Console.WriteLine("Press any key to return to the menu.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Task not found with that ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        public static void CreateTask(TaskManager taskManager)
        {
            Console.Clear();
            Console.WriteLine("--- Title ---");
            string title = Console.ReadLine() ?? "Untitled Task";
            Console.Clear();
            Console.WriteLine("--- Description ---");
            string description = Console.ReadLine() ?? "";
            Console.Clear();
            Console.WriteLine("--- Status ---");
            Status status = GetStatusFromUser();
            Console.Clear();
            Console.WriteLine("--- Deadline ---");
            DateTime deadline = GetDeadlineFromUser();
            Console.Clear();
            Console.WriteLine("--- Priority ---");
            Priority priority = GetPriorityFromUser();
            Console.Clear();

            // Call the TaskManager to do the actual work
            taskManager.CreateTask(title, description, status, deadline, priority);

            Console.WriteLine("Task created successfully!");
            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        public static void ViewTasks(TaskManager taskManager, bool pause = true)
        {
            List<KanbanTask> taskList = taskManager.GetTasks();

            Console.WriteLine("--- All Tasks ---");
            if (taskList.Count == 0)
            {
                Console.WriteLine("No tasks to display. Please create a task first.");
                if (!pause)
                {
                    Console.WriteLine("Press any key to return.");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                for (int i = 0; i < taskList.Count; i++)
                {
                    KanbanTask task = taskList[i];

                    Console.WriteLine($"ID: {task.Id}");
                    Console.WriteLine($"Title: {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Status: {task.Status}");
                    Console.WriteLine($"Priority: {task.Priority}");
                    Console.WriteLine($"Deadline: {task.Deadline.ToShortDateString()}");
                    Console.WriteLine("-----------------");
                }
            }

            if (pause)
            {
                Console.WriteLine("Press any key to return to the menu.");
                Console.ReadKey();
            }
        }

        public static void EditTask(TaskManager taskManager)
        {
            Console.Clear();
            ViewTasks(taskManager, pause: false);

            Console.WriteLine("\nEnter the ID of the task you want to edit:");
            Console.WriteLine("Or press Enter to cancel.");
            if (int.TryParse(Console.ReadLine(), out int inputId))
            {
                KanbanTask taskToEdit = taskManager.GetTaskById(inputId);

                if (taskToEdit != null)
                {
                    Console.WriteLine($"Editing Task: {taskToEdit.Title}");
                    Console.WriteLine("Which field would you like to update?");
                    Console.WriteLine("1. Title");
                    Console.WriteLine("2. Description");
                    Console.WriteLine("3. Status");
                    Console.WriteLine("4. Priority");
                    Console.WriteLine("5. Deadline");
                    Console.WriteLine("6. Cancel");

                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("Enter new Title:");
                                string newTitle = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newTitle))
                                    taskToEdit.Title = newTitle;
                                break;
                            case 2:
                                Console.WriteLine("Enter new Description:");
                                taskToEdit.Description = Console.ReadLine();
                                break;
                            case 3:
                                Console.WriteLine("Select new Status:");
                                taskToEdit.Status = GetStatusFromUser();
                                break;
                            case 4:
                                Console.WriteLine("Select new Priority:");
                                taskToEdit.Priority = GetPriorityFromUser();
                                break;
                            case 5:
                                Console.WriteLine("Select new Deadline:");
                                taskToEdit.Deadline = GetDeadlineFromUser();
                                break;
                            default:
                                Console.WriteLine("Edit cancelled.");
                                return;
                        }
                        Console.WriteLine("Task updated successfully!");
                        Console.WriteLine("Press enter to return to the menu");
                    }
                    else
                    {
                        Console.WriteLine("Task not found with that ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID format.");
                }
            }
        }
    }
}
