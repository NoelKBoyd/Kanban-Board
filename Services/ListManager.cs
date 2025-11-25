using Kanban_Board.Classes;
using Kanban_Board.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                Tasks = new List<KanbanTask>()
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

                    //write number of Tasks in this exact List
                    writer.Write(list.Tasks.Count);

                    foreach (var task in list.Tasks)
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

                        KanbanList newList = new KanbanList
                        {
                            Id = listId,
                            Title = listTitle,
                            Description = listDesc,
                            Tasks = new List<KanbanTask>()
                        };

                        int taskCount = reader.ReadInt32();

                        //Read each Task from the list
                        for (int j = 0; j < taskCount; j++)
                        {
                            int taskId = reader.ReadInt32();
                            string tTitle = reader.ReadString();
                            string tDesc = reader.ReadString();
                            Status tStatus = (Status)reader.ReadInt32();
                            DateTime tDeadline = DateTime.FromBinary(reader.ReadInt64());
                            Priority tPriority = (Priority)reader.ReadInt32();

                            KanbanTask newTask = new KanbanTask(taskId, tTitle, tDesc, tStatus, tDeadline, tPriority);
                            newList.Tasks.Add(newTask);
                        }

                        _lists.Add(newList);
                    }

                    //Update _nextListId
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