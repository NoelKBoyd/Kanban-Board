using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanList : WorkItem
    {
        public List<KanbanTask> Tasks { get; set; } = new List<KanbanTask>();

        public override Status Status
        {
            get
            {
                if (Tasks.Count == 0) return Status.ToDo;
                if (Tasks.All(t => t.Status == Status.Done)) return Status.Done;
                if (Tasks.Any(t => t.Status == Status.InProgress)) return Status.InProgress;
                return Status.ToDo;
            }
        }
    }
}