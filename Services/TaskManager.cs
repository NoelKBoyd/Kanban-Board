using Kanban_Board.Classes;
using Kanban_Board.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kanban_Board.Services
{
    internal class TaskManager
    {
        private Dictionary<int, KanbanTask> _tasks = new Dictionary<int, KanbanTask>();

        private int _nextTaskId = 1;

        public Dictionary<int, KanbanTask> GetTasks()
        {
            return _tasks;
        }

        public void CreateTask(string title, string description, Status status, DateTime deadline, Priority priority)
        {
            KanbanTask newTask = new KanbanTask(_nextTaskId++, title, description, status, deadline, priority);

            _tasks.Add(newTask.Id, newTask);
        }

        public KanbanTask? GetTaskById(int id)
        {
            //this is now way faster compared to searching a list
            if (_tasks.TryGetValue(id, out KanbanTask? task))
            {
                return task;
            }
            return null;
        }

        public void DeleteTask(KanbanTask task)
        {
            // remove requires the key (Id)
            if (_tasks.ContainsKey(task.Id))
            {
                _tasks.Remove(task.Id);
            }
        }

        // --- BINARY SAVE/LOAD IMPLEMENTATION ---

        public string GetFileName(User user)
        {
            return $"{user.Username}_tasks.bin";
        }

        public void SaveTasks(User user)
        {
            string fileName = GetFileName(user);
            using (FileStream stream = File.Create(fileName))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(_tasks.Count);

                foreach (var task in _tasks.Values)
                {
                    writer.Write(task.Id);
                    writer.Write(task.Title ?? "");
                    writer.Write(task.Description ?? "");
                    writer.Write((int)task.Status);
                    writer.Write(task.Deadline.ToBinary());
                    writer.Write((int)task.Priority);
                }
            }
            Console.WriteLine($"Tasks saved successfully to {fileName}");
        }

        public void LoadTasks(User user)
        {
            string fileName = $"{user.Username}_tasks.bin";
            if (!File.Exists(fileName))
            {
                _tasks = new Dictionary<int, KanbanTask>();
                _nextTaskId = 1;
                return;
            }

            try
            {
                using (FileStream stream = File.OpenRead(fileName))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    _tasks.Clear();
                    int count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        int id = reader.ReadInt32();
                        string title = reader.ReadString();
                        string description = reader.ReadString();
                        Status status = (Status)reader.ReadInt32();
                        DateTime deadline = DateTime.FromBinary(reader.ReadInt64());
                        Priority priority = (Priority)reader.ReadInt32();

                        KanbanTask loadedTask = new KanbanTask(id, title, description, status, deadline, priority);

                        //add using ID as key
                        _tasks.Add(loadedTask.Id, loadedTask);
                    }

                    if (_tasks.Count > 0)
                    {
                        //look at the keys property to find the max ID
                        _nextTaskId = _tasks.Keys.Max() + 1;
                    }
                    else
                    {
                        _nextTaskId = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
                _tasks = new Dictionary<int, KanbanTask>();
                _nextTaskId = 1;
            }
        }
    }
}