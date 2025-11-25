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

        public void AddListToBoard(int boardIndex, KanbanList list)
        {
            if (boardIndex >= 0 && boardIndex < _boards.Count)
            {
                _boards[boardIndex].Lists.Add(list);
            }
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

                    writer.Write(board.Lists.Count);

                    foreach (var list in board.Lists) //write list data
                    {

                        writer.Write(list.Id);
                        writer.Write(list.Title ?? "");
                        writer.Write(list.Description ?? "");
                        writer.Write((int)list.Status);

                        writer.Write(list.Tasks.Count);

                        foreach (var task in list.Tasks) //write the tasks in the list
                        {
                            writer.Write(task.Id);
                            writer.Write(task.Title ?? "");
                            writer.Write(task.Description ?? "");
                            writer.Write((int)task.Status);
                            writer.Write(task.Deadline.ToBinary());
                            writer.Write((int)task.Priority);
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

                        KanbanBoard newBoard = new KanbanBoard
                        {
                            Id = boardId,
                            Title = boardTitle,
                            Description = boardDesc,
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
                            DateTime listDeadline = DateTime.FromBinary(reader.ReadInt64());
                            Priority listPriority = (Priority)reader.ReadInt32();

                            KanbanList newList = new KanbanList
                            {
                                Id = listId,
                                Title = listTitle,
                                Description = listDesc,
                                Status = listStatus,
                                Tasks = new List<KanbanTask>()
                            };

                            //read number of tasks in this list
                            int taskCount = reader.ReadInt32();

                            for (int k = 0; k < taskCount; k++)
                            {
                                //read task data
                                int taskId = reader.ReadInt32();
                                string tTitle = reader.ReadString();
                                string tDesc = reader.ReadString();
                                Status tStatus = (Status)reader.ReadInt32();
                                DateTime tDeadline = DateTime.FromBinary(reader.ReadInt64());
                                Priority tPriority = (Priority)reader.ReadInt32();

                                KanbanTask newTask = new KanbanTask(taskId, tTitle, tDesc, tStatus, tDeadline, tPriority);
                                newList.Tasks.Add(newTask);
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