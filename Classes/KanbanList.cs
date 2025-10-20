using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanList
    {
        public required string title { get; set; }
        public string? description { get; set; }
        public DateTime deadline { get; set; }
        public Priority priority { get; set; }
        public List<KanbanTask> tasks { get; set; } = new List<KanbanTask>();

        public Status status
        {
            get
            {
                if (tasks.Count == 0)
                {
                    return Status.ToDo;
                }
                if (tasks.All(t => t.status == Status.Done))
                {
                    return Status.Done;
                }
                if (tasks.Any(t => t.status == Status.InProgress))
                {
                    return Status.InProgress;
                }
                return Status.ToDo;
            }
        }
    }
}
