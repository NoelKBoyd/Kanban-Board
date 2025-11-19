using Kanban_Board.Classes;
using Kanban_Board.Enums;

namespace Kanban_Board.Services
{
    internal class TaskManager
    {
        private List<KanbanTask> _tasks = new List<KanbanTask>();
        public List<KanbanTask> GetTasks()
        {
            return _tasks;
        }

        public void CreateTask(string title, string description, Status status, DateTime deadline, Priority priority)
        {
            KanbanTask newTask = new KanbanTask(title, description, status, deadline, priority);
            _tasks.Add(newTask);
        }

        public void DeleteTask(KanbanTask task)
        {
            _tasks.Remove(task);
        }

        private void SaveTasks()
        {
            using (FileStream stream = File.Create("Tasks.bin"))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(_tasks.Count);
                foreach (var task in _tasks)
                {
                    writer.Write(task.Title);
                    writer.Write(task.Status.ToString());
                    writer.Write(task.Description);
                    writer.Write(task.Deadline.ToBinary());
                    writer.Write(task.Priority.ToString());
                }
            }
        }

    }
}
