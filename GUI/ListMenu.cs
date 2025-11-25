using Kanban_Board.Services;
using static Kanban_Board.GUI.InputHelper;

namespace Kanban_Board.GUI
{
    internal class ListMenu
    {
        public static void CreateList(ListManager boardManager)
        {
            Console.Clear();
            Console.WriteLine("--- Create New List ---");
            Console.WriteLine("Enter List Title:");
            string title = Console.ReadLine();

            boardManager.CreateList(title);
            Console.WriteLine("List created!");
            Console.ReadKey();
        }

        public static void ViewLists(ListManager boardManager)
        {
            var lists = boardManager.GetLists();
            int index = 0;
            foreach (var list in lists)
            {
                Console.WriteLine($"{index}. {list.Title} ({list.Tasks.Count} tasks)");
                index++;
            }
            Console.ReadKey();
        }
    }
}
