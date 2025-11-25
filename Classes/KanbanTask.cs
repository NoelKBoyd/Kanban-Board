using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class KanbanTask : WorkItem
    {
        public KanbanTask(int id, string title, string description, Status status, DateTime deadline, Priority priority)
        {
            Id = id;
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

        public bool IsOverdue()
        {
            return Status != Status.Done && DateTime.Now > Deadline;
        }

        public string GetTimeRemaining()
        {
            if (Status == Status.Done) return "Completed";

            TimeSpan remaining = Deadline - DateTime.Now;

            if (remaining.TotalDays < 0)
                return $"Overdue by {Math.Abs(remaining.Days)} days";
            else if (remaining.TotalDays < 1)
                return "Due today";
            else
                return $"{remaining.Days} days remaining";
        }
    }
}