using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanList : KanbanEntity
    {
        public List<int> TaskIds { get; set; } = new List<int>(); //change to taskIds instead of objects

        public override Status Status { get; set; } = Status.ToDo; // required taskManager access before now its a property, defaults to be status todo

        public override string GetDetails()
        {
            return $"ID: {Id} | List: {Title} | Status: {Status} | Tasks: {TaskIds.Count}";
        }
    }
}