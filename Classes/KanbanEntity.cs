using Kanban_Board.Enums; 

namespace Kanban_Board.Classes
{
    internal abstract class KanbanEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public virtual Status Status { get; set; }
    }
}

//Kanbantask, kanbanlist and kanbanboard inherit from this class