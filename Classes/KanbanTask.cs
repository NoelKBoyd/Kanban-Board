using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanTask : WorkItem
    {
        public KanbanTask(string title, string description, Status status, DateTime deadline, Priority priority)
        {
            Title = title;
            Description = description;
            Status = status;
            Deadline = deadline;
            Priority = priority;
        }

        public void SetStatus(Status newStatus)
        {
            Status = newStatus;
        }

        public void MarkAsDone()
        {
            Status = Status.Done;
        }

        public void SetPriority(Priority newPriority)
        {
            Priority = newPriority;
        }
    }
}