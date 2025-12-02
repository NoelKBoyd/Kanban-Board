using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanBoard : KanbanEntity
    {
        public List<KanbanList> Lists { get; set; } = new List<KanbanList>();

        public override Status Status { get; set; } = Status.ToDo; //previously used KanbanList dynamic status now its just a property

        public override string GetDetails()
        {
            return $"ID: {Id} | Board: {Title} | Status: {Status} | Lists: {Lists.Count}";
        }
    }
}