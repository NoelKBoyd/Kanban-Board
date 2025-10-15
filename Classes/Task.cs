using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class Task
    {
        public required string title { get; set; }
        public string? description { get; set; }
        public Status status { get; set; }
        public DateTime deadline { get; set; }
        public Priority priority { get; set; }
        public bool isCompleted { get; set; } = false;

        public Task(string title, string description, Status status, DateTime deadline, Priority priority)
        {
            this.title = title;
            this.description = description;
            this.status = status;
            this.deadline = deadline;
            this.priority = priority;
        }
    }
}
