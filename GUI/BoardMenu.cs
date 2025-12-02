using Kanban_Board.Classes;
using Kanban_Board.Services;

namespace Kanban_Board.GUI
{
    internal class BoardMenu
    {
        public static void ViewBoards(BoardManager boardManager, bool pause = true)
        {
            Console.Clear();
            Dictionary<int, KanbanBoard> boardList = boardManager.GetBoards();
            Console.WriteLine("--- All Boards ---");
            if (boardList.Count == 0)
            {
                Console.WriteLine("No boards to display. Please create a board first.");
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
                foreach (var board in boardList.Values)
                {
                    Console.WriteLine($"ID: {board.Id}");
                    Console.WriteLine($"Title: {board.Title}");
                    Console.WriteLine($"Description: {board.Description}");
                    Console.WriteLine($"Status: {board.Status}");
                    Console.WriteLine("-----------------");
                }
            }

            if (!pause) return;

            while (true)
            {
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
                    }
                    else
                    {
                        Console.WriteLine($"Board with ID {boardId} not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format. Please enter a number.");
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
            boardManager.CreateBoard(title, description);
            Console.WriteLine("Board created!");
            Thread.Sleep(1000);
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
                    Console.WriteLine($"Editing List: {boardToEdit.Title}");
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
                                    boardToEdit.Title = newTitle;
                                break;
                            case 2:
                                Console.WriteLine("");
                                Console.WriteLine("Enter new Description:");
                                boardToEdit.Description = Console.ReadLine();
                                break;
                            default:
                                Console.WriteLine("");
                                Console.WriteLine("Edit cancelled.");
                                return;
                        }
                        Console.WriteLine("Board updated successfully!");
                        Console.WriteLine("Press enter to return to the menu");
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Board not found with that ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID format.");
                }
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
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Board not found with that ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
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
                return;
            }

            var allLists = listManager.GetLists();
            if (allLists.Count == 0)
            {
                Console.WriteLine("There are no available lists to move.");
                Thread.Sleep(1000);
                Console.Clear();
                return;
            }

            Console.WriteLine("\nAvailable Lists:");
            //allLists is a Dictionary so iterate over .Values
            foreach (var list in allLists.Values)
            {
                Console.WriteLine($"ID: {list.Id} | Title: {list.Title} | Description: {list.Description}");
            }

            Console.WriteLine("\nEnter the IDs of the lists to move/add (comma separated):");
            string? listInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(listInput)) return;

            string[] listIds = listInput.Split(',');
            int successCount = 0;

            foreach (var idStr in listIds)
            {
                if (int.TryParse(idStr.Trim(), out int listId))
                {
                    //verify the list exists in the ListManager
                    KanbanList? listObj = listManager.GetListById(listId);

                    if (listObj != null)
                    {
                        //pass the IDs
    
                        bool success = boardManager.AddOrMoveList(listObj.Id, targetBoardId);

                        if (success) successCount++;
                    }
                    else
                    {
                        Console.WriteLine($"List ID {listId} does not exist in the database.");
                    }
                }
            }

            if (successCount > 0)
            {
                Console.WriteLine($"\nSuccessfully processed {successCount} list(s)!");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}

