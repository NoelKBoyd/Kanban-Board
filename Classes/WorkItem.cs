using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal abstract class WorkItem : KanbanEntity 
    {
        public DateTime Deadline { get; set; }
        public Priority Priority { get; set; }
    }
}



