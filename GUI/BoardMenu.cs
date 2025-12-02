using Kanban_Board.Classes;
using Kanban_Board.Services;

namespace Kanban_Board.GUI
{
    internal class BoardMenu
    {
        public static void ViewBoards(BoardManager boardManager, bool pause = true)
        {
            Dictionary<int, KanbanBoard> boardList = boardManager.GetBoards();

            void PrintAllBoards()
            {
                Console.WriteLine("--- All Boards ---");
                if (boardList.Count == 0)
                {
                    Console.WriteLine("No boards to display. Please create a board first.");
                }
                else
                {
                    var sortedBoards = boardList.Values.OrderBy(l => l.Status);

                    foreach (KanbanBoard board in sortedBoards)
                    {
                        Console.WriteLine(board.GetDetails());
                        Console.WriteLine("-----------------");
                    }
                }
            }

            if (!pause)
            {
                Console.Clear();
                PrintAllBoards();
                return;
            }

            while (true)
            {
                Console.Clear();
                PrintAllBoards();

                if (boardList.Count == 0)
                {
                    Console.WriteLine("\nPress any key to return.");
                    Console.ReadKey();
                    break;
                }

                Console.WriteLine("\nOptions:");
                Console.WriteLine("- Enter a [board ID] to view its lists");
                Console.WriteLine("- Press [Enter] to return to the menu");
                Console.Write("> ");

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                if (int.TryParse(input, out int boardId))
                {
                    KanbanBoard? selectedBoard = boardManager.GetBoardById(boardId);

                    if (selectedBoard != null)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\n--- Lists in '{selectedBoard.Title}' ---");
                        Console.ResetColor();

                        if (selectedBoard.Lists.Count == 0)
                        {
                            Console.WriteLine("  (This board is empty)");
                        }
                        else
                        {
                            foreach (var list in selectedBoard.Lists)
                            {
                                Console.WriteLine($"ID: {list.Id}");
                                Console.WriteLine($"Title: {list.Title}");
                                Console.WriteLine($"Description: {list.Description}");
                                Console.WriteLine($"Status: {list.Status}");
                                Console.WriteLine("-----------------");
                            }
                        }
                        Console.WriteLine("-----------------------");

                        Console.WriteLine("\nPress any key to return to the board view...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"\nBoard with ID {boardId} not found.");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid format. Please enter a number.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
            }

            Console.Clear();
        }

        public static void CreateBoard(BoardManager boardManager)
        {
            Console.Clear();
            string title = "";
            while (string.IsNullOrWhiteSpace(title))
            {
                Console.Clear();
                Console.WriteLine("--- Title ---");

                string? input = Console.ReadLine();

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
            boardManager.CreateBoard(title, description);
            Console.WriteLine("Board created successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void EditBoard(BoardManager boardManager)
        {
            Console.Clear();
            ViewBoards(boardManager, pause: false);

            Console.WriteLine("\nEnter the ID of the board you want to edit:");
            Console.WriteLine("Or press Enter to cancel.");
            Console.WriteLine("");

            if (int.TryParse(Console.ReadLine(), out int inputId))
            {
                KanbanBoard boardToEdit = boardManager.GetBoardById(inputId);

                if (boardToEdit != null)
                {
                    Console.WriteLine("");
                    Console.WriteLine($"Editing Board: {boardToEdit.Title}");
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
                                string? newTitle = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newTitle))
                                    boardToEdit.Title = newTitle;
                                break;
                            case 2:
                                Console.WriteLine("");
                                Console.WriteLine("Enter new Description:");
                                boardToEdit.Description = Console.ReadLine() ?? "";
                                break;
                            default:
                                Console.WriteLine("");
                                Console.WriteLine("Edit cancelled.");
                                return;
                        }
                        Console.WriteLine("Board updated successfully!");
                        Console.WriteLine("Press enter to return to the menu");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Board not found with that ID.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
            }
        }

        public static void DeleteBoard(BoardManager boardManager)
        {
            Console.Clear();
            ViewBoards(boardManager, pause: false);
            Console.WriteLine("\nEnter the ID of the board you want to delete:");

            string? input = Console.ReadLine();
            if (int.TryParse(input, out int response))
            {
                KanbanBoard boardToDelete = boardManager.GetBoardById(response);
                if (boardToDelete != null)
                {
                    boardManager.DeleteBoard(boardToDelete);
                    Console.WriteLine("Board deleted successfully!");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Board not found with that ID.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
            }
        }

        public static void MoveListsToBoard(BoardManager boardManager, ListManager listManager)
        {
            Console.Clear();
            ViewBoards(boardManager, pause: false);

            Console.WriteLine("\n--- Move/Add Lists ---");
            Console.WriteLine("Enter the ID of the DESTINATION Board:");

            if (!int.TryParse(Console.ReadLine(), out int targetBoardId))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            if (boardManager.GetBoardById(targetBoardId) == null)
            {
                Console.WriteLine("Destination Board not found.");
                Console.ReadKey();
                return;
            }

            var allLists = listManager.GetLists();
            if (allLists.Count == 0)
            {
                Console.WriteLine("There are no available lists to move.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nAvailable Lists:");
            foreach (var list in allLists.Values)
            {
                Console.WriteLine($"ID: {list.Id} | Title: {list.Title} | Description: {list.Description}");
            }

            Console.WriteLine("\nEnter the IDs of the lists to move/add (comma separated):");
            string? listInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(listInput)) return;

            string[] listIds = listInput.Split(',');
            int successCount = 0;
            List<string> errors = new List<string>();

            foreach (var idStr in listIds)
            {
                if (int.TryParse(idStr.Trim(), out int listId))
                {
                    KanbanList? listObj = listManager.GetListById(listId);

                    if (listObj != null)
                    {
                        bool success = boardManager.AddOrMoveList(listObj.Id, targetBoardId);

                        if (success)
                            successCount++;
                        else
                            errors.Add($"ID {listId}: Failed (Already in board or error moving)");
                    }
                    else
                    {
                        errors.Add($"ID {listId}: Not found in database");
                    }
                }
                else
                {
                    errors.Add($"'{idStr}' is not a valid number");
                }
            }

            Console.WriteLine($"\nSuccessfully processed {successCount} list(s)!");

            if (errors.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nErrors encountered:");
                foreach (var error in errors) Console.WriteLine("- " + error);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}