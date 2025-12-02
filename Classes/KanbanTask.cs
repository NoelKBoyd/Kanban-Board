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

        public override string GetDetails()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"ID: {Id} | {Title} ({Priority})");
            sb.AppendLine($"Description: {Description}");
            sb.AppendLine($"Status: {Status}");
            sb.AppendLine($"Due: {Deadline.ToShortDateString()} ({GetTimeRemaining()})");
            return sb.ToString();
        }

        public string GetTimeRemaining()
        {
            if (Status == Status.Done) return "Completed";

            TimeSpan remaining = Deadline - DateTime.Now;

            if (remaining.TotalDays < 0)
                return $"Overdue by {Math.Abs(remaining.Days)} days";
            else if (Math.Ceiling(remaining.TotalDays) <= 1)
                return "Due today/tomorrow";
            else
                return $"{remaining.Days} days remaining";
        }
    }
}