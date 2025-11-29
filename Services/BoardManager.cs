using Kanban_Board.Classes;
using Kanban_Board.Enums;

namespace Kanban_Board.Services
{
    internal class BoardManager
    {
        private List<KanbanBoard> _boards = new List<KanbanBoard>();
        private int _nextBoardId = 1;

        public void CreateBoard(string title, string description)
        {
            KanbanBoard newBoard = new KanbanBoard
            {
                Id = _nextBoardId++,
                Title = title,
                Description = description,
                Lists = new List<KanbanList>() // boards initialize with empty Lists
            };
            _boards.Add(newBoard);
        }

        public List<KanbanBoard> GetBoards()
        {
            return _boards;
        }

        public KanbanBoard? GetBoardById(int id)
        {
            return _boards.FirstOrDefault(b => b.Id == id);
        }

        public bool AddOrMoveList(KanbanList listToMove, int targetColumnId)
        {
            var targetColumn = _boards.FirstOrDefault(l => l.Id == targetColumnId);
            if (targetColumn == null)
            {
                Console.WriteLine($"Error: Target board with ID {targetColumnId} not found.");
                return false;
            }

            var currentOwnerList = _boards.FirstOrDefault(l => l.Lists.Any(t => t.Id == listToMove.Id));

            if (currentOwnerList != null)
            {
                if (currentOwnerList == targetColumn)
                {
                    Console.WriteLine("Task is already in this list.");
                    return false;
                }

                currentOwnerList.Lists.Remove(listToMove);
            }

            targetColumn.Lists.Add(listToMove);

            return true;
        }

        public void DeleteBoard(KanbanBoard board)
        {
            _boards.Remove(board);
        }

        // --- BINARY SAVE/LOAD IMPLEMENTATION ---

        public void SaveBoards(User user)
        {
            string fileName = $"{user.Username}_boards.bin";
            using (FileStream stream = File.Create(fileName))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(_boards.Count);

                foreach (var board in _boards)
                {
                    //write board data
                    writer.Write(board.Id);
                    writer.Write(board.Title ?? "Untitled");
                    writer.Write(board.Description ?? "");
                    writer.Write((int)board.Status);

                    writer.Write(board.Lists.Count);

                    foreach (var list in board.Lists) //write list data
                    {
                        writer.Write(list.Id);
                        writer.Write(list.Title ?? "");
                        writer.Write(list.Description ?? "");
                        writer.Write((int)list.Status);
                        writer.Write(list.TaskIds.Count);//write number of Task Ids
                        foreach (var taskId in list.TaskIds)//write only the Task Ids
                        {
                            writer.Write(taskId);
                        }
                    }
                }
            }
            Console.WriteLine($"Boards saved successfully to {fileName}");
        }

        public void LoadBoards(User user)
        {
            string fileName = $"{user.Username}_boards.bin";

            if (!File.Exists(fileName))
            {
                _boards = new List<KanbanBoard>();
                _nextBoardId = 1;
                return;
            }

            try
            {
                using (FileStream stream = File.OpenRead(fileName))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    _boards.Clear();

                    int boardCount = reader.ReadInt32();

                    for (int i = 0; i < boardCount; i++)
                    {
                        int boardId = reader.ReadInt32();
                        string boardTitle = reader.ReadString();
                        string boardDesc = reader.ReadString();
                        Status boardStatus = (Status)reader.ReadInt32();

                        KanbanBoard newBoard = new KanbanBoard
                        {
                            Id = boardId,
                            Title = boardTitle,
                            Description = boardDesc,
                            Status = boardStatus,
                            Lists = new List<KanbanList>()
                        };

                        int listCount = reader.ReadInt32();

                        for (int j = 0; j < listCount; j++)
                        {
                            //read list data
                            int listId = reader.ReadInt32();
                            string listTitle = reader.ReadString();
                            string listDesc = reader.ReadString();
                            Status listStatus = (Status)reader.ReadInt32();

                            KanbanList newList = new KanbanList
                            {
                                Id = listId,
                                Title = listTitle,
                                Description = listDesc,
                                Status = listStatus,
                                TaskIds = new List<int>() // use TaskIds
                            };

                            
                            int taskIdCount = reader.ReadInt32();//read number of tasks in this list

                            for (int k = 0; k < taskIdCount; k++)
                            {
                                int taskId = reader.ReadInt32();//read only the task ID
                                newList.TaskIds.Add(taskId);
                            }

                            newBoard.Lists.Add(newList);
                        }

                        _boards.Add(newBoard);
                    }

                    //update _nextBoardId to avoid duplicates
                    if (_boards.Count > 0)
                    {
                        _nextBoardId = _boards.Max(b => b.Id) + 1;
                    }
                    else
                    {
                        _nextBoardId = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading boards: {ex.Message}");
                _boards = new List<KanbanBoard>();
                _nextBoardId = 1;
            }
        }
    }
}