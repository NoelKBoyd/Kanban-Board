using Kanban_Board.Classes;
using Kanban_Board.Enums;

namespace Kanban_Board.Services
{
    internal class TaskManager
    {
        private List<KanbanTask> _tasks = new List<KanbanTask>();

        private int _nextId = 1;  //unique IDs

        public List<KanbanTask> GetTasks()
        {
            return _tasks;
        }

        public void CreateTask(string title, string description, Status status, DateTime deadline, Priority priority)
        {
            KanbanTask newTask = new KanbanTask(_nextId++,title, description, status, deadline, priority);
            _tasks.Add(newTask);
        }

        public KanbanTask? GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void DeleteTask(KanbanTask task)
        {
            _tasks.Remove(task);
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

                foreach (var task in _tasks)
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
                _tasks = new List<KanbanTask>();
                _nextId = 1;
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

                        _tasks.Add(new KanbanTask(id, title, description, status, deadline, priority));
                    }

                    if (_tasks.Count > 0)
                    {
                        _nextId = _tasks.Max(t => t.Id) + 1;
                    }
                    else
                    {
                        _nextId = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
                _tasks = new List<KanbanTask>();
                _nextId = 1;
            }
        }
    }
}