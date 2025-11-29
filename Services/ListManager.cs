using Kanban_Board.Classes;
using Kanban_Board.Enums;

namespace Kanban_Board.Services
{
    internal class ListManager
    {
        private List<KanbanList> _lists = new List<KanbanList>();
        private int _nextListId = 1; //unique IDs

        public void CreateList(string title, string description)
        {
            KanbanList newList = new KanbanList
            {
                Id = _nextListId++,
                Title = title,
                Description = description,
                TaskIds = new List<int>(),
            };
            _lists.Add(newList);
        }

        public List<KanbanList> GetLists()
        {
            return _lists;
        }

        public KanbanList? GetListById(int id)
        {
            return _lists.FirstOrDefault(l => l.Id == id);
        }

        public void DeleteList(KanbanList list)
        {
            _lists.Remove(list);
        }

        public bool AddOrMoveTask(int taskId, int targetColumnId)
        {
            var targetColumn = _lists.FirstOrDefault(l => l.Id == targetColumnId);
            if (targetColumn == null)
            {
                Console.WriteLine($"Error: Target list with ID {targetColumnId} not found.");
                return false;
            }

            var currentOwnerList = _lists.FirstOrDefault(l => l.TaskIds.Any(id => id == taskId));

            if (currentOwnerList != null)
            {
                if (currentOwnerList == targetColumn)
                {
                    Console.WriteLine("Task is already in this list.");
                    return false;
                }

                currentOwnerList.TaskIds.Remove(taskId);
            }

            targetColumn.TaskIds.Add(taskId);

            return true;
        }

        // --- BINARY SAVE/LOAD IMPLEMENTATION ---

        public void SaveLists(User user)
        {
            string fileName = $"{user.Username}_lists.bin";
            using (FileStream stream = File.Create(fileName))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                //write total number of Lists
                writer.Write(_lists.Count);

                foreach (var list in _lists)
                {
                    //write List data
                    writer.Write(list.Id);
                    writer.Write(list.Title ?? "Untitled");
                    writer.Write(list.Description ?? "");
                    writer.Write((int)list.Status);
                    writer.Write(list.TaskIds.Count);
                    foreach (var taskId in list.TaskIds)
                    {
                        writer.Write(taskId);
                    }
                }
            }
            Console.WriteLine($"Lists saved successfully to {fileName}");
        }

        public void LoadLists(User user)
        {
            string fileName = $"{user.Username}_lists.bin";

            if (!File.Exists(fileName))
            {
                _lists = new List<KanbanList>();
                _nextListId = 1;
                return;
            }

            try
            {
                using (FileStream stream = File.OpenRead(fileName))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    _lists.Clear();

                    //read total number of Lists
                    int listCount = reader.ReadInt32();

                    for (int i = 0; i < listCount; i++)
                    {
                        int listId = reader.ReadInt32();
                        string listTitle = reader.ReadString();
                        string listDesc = reader.ReadString();
                        Status listStatus = (Status)reader.ReadInt32(); // Read status

                        KanbanList newList = new KanbanList
                        {
                            Id = listId,
                            Title = listTitle,
                            Description = listDesc,
                            Status = listStatus,
                            TaskIds = new List<int>() // Initialize TaskIds
                        };

                        //read number of Task Ids
                        int taskIdCount = reader.ReadInt32();

                        //read each Task ID from the list
                        for (int j = 0; j < taskIdCount; j++)
                        {
                            int taskId = reader.ReadInt32();
                            newList.TaskIds.Add(taskId);
                        }

                        _lists.Add(newList);
                    }

                    if (_lists.Count > 0)
                    {
                        _nextListId = _lists.Max(l => l.Id) + 1;
                    }
                    else
                    {
                        _nextListId = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading lists: {ex.Message}");
                _lists = new List<KanbanList>();
                _nextListId = 1;
            }
        }
    }
}