using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanTask
    {
        public string title { get; set; }
        public string? description { get; set; }
        public Status status { get; set; }
        public DateTime deadline { get; set; }
        public Priority priority { get; set; }

        public KanbanTask(string title, string description, Status status, DateTime deadline, Priority priority)
        {
            this.title = title;
            this.description = description;
            this.status = status;
            this.deadline = deadline;
            this.priority = priority;
        }
    }
}
