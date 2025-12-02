using Kanban_Board.Classes;
using Kanban_Board.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kanban_Board.Services
{
    internal class BoardManager
    {
        private Dictionary<int, KanbanBoard> _boards = new Dictionary<int, KanbanBoard>();
        private int _nextBoardId = 1;

        public void CreateBoard(string title, string description)
        {
            KanbanBoard newBoard = new KanbanBoard
            {
                Id = _nextBoardId++,
                Title = title,
                Description = description,
                Lists = new List<KanbanList>()
            };
            _boards.Add(newBoard.Id, newBoard);
        }

        public Dictionary<int, KanbanBoard> GetBoards()
        {
            return _boards;
        }

        public KanbanBoard? GetBoardById(int id)
        {
            if (_boards.TryGetValue(id, out KanbanBoard? board))
            {
                return board;
            }
            return null;
        }

        public bool AddOrMoveList(int listId, int targetBoardId)
        {
            if (!_boards.TryGetValue(targetBoardId, out var targetBoard))
            {
                Console.WriteLine($"Error: Target board with ID {targetBoardId} not found.");
                return false;
            }

            KanbanList? listToMove = null;
            KanbanBoard? currentOwnerBoard = null;


            foreach (var board in _boards.Values)
            {
                var foundList = board.Lists.FirstOrDefault(l => l.Id == listId);
                if (foundList != null)
                {
                    listToMove = foundList;
                    currentOwnerBoard = board;
                    break;
                }
            }

            if (listToMove == null)
            {
                Console.WriteLine($"Error: List with ID {listId} not found in any board.");
                return false;
            }

            if (currentOwnerBoard != null)
            {
                if (currentOwnerBoard == targetBoard)
                {
                    Console.WriteLine("List is already in this board.");
                    return false;
                }

                currentOwnerBoard.Lists.Remove(listToMove);
            }

            targetBoard.Lists.Add(listToMove);

            return true;
        }

        public void DeleteBoard(KanbanBoard board)
        {
            if (_boards.ContainsKey(board.Id))
            {
                _boards.Remove(board.Id);
            }
        }

        // --- BINARY SAVE/LOAD IMPLEMENTATION ---

        public void SaveBoards(User user)
        {
            string fileName = $"{user.Username}_boards.bin";
            using (FileStream stream = File.Create(fileName))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(_boards.Count);

                foreach (var board in _boards.Values)
                {
                    writer.Write(board.Id);
                    writer.Write(board.Title ?? "Untitled");
                    writer.Write(board.Description ?? "");
                    writer.Write((int)board.Status);

                    writer.Write(board.Lists.Count);

                    foreach (var list in board.Lists)
                    {
                        writer.Write(list.Id);
                        writer.Write(list.Title ?? "");
                        writer.Write(list.Description ?? "");
                        writer.Write((int)list.Status);
                        writer.Write(list.TaskIds.Count);

                        foreach (var taskId in list.TaskIds)
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
                _boards = new Dictionary<int, KanbanBoard>();
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
                                TaskIds = new List<int>()
                            };

                            int taskIdCount = reader.ReadInt32();

                            for (int k = 0; k < taskIdCount; k++)
                            {
                                int taskId = reader.ReadInt32();
                                newList.TaskIds.Add(taskId);
                            }

                            newBoard.Lists.Add(newList);
                        }

                        _boards.Add(newBoard.Id, newBoard);
                    }

                    if (_boards.Count > 0)
                    {
                        _nextBoardId = _boards.Keys.Max() + 1;
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
                _boards = new Dictionary<int, KanbanBoard>();
                _nextBoardId = 1;
            }
        }
    }
}