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
    }
}