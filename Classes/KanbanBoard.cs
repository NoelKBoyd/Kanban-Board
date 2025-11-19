using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanBoard : KanbanEntity
    {
        public List<KanbanList> Lists { get; set; } = new List<KanbanList>();

        public override Status Status
        {
            get
            {
                if (Lists.Count == 0) return Status.ToDo;
                if (Lists.All(l => l.Status == Status.Done)) return Status.Done;
                if (Lists.Any(l => l.Status == Status.InProgress)) return Status.InProgress;
                return Status.ToDo;
            }
            set { }
        }
    }
}