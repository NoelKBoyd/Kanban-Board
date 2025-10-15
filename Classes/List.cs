using Kanban_Board.Enums;

namespace Kanban_Board.Classes
{
    internal class List
    {
        public required string name { get; set; }
        public string? description { get; set; }
        public Status status { get; set; }
        public DateTime deadline { get; set; }
        public Priority priority { get; set; }
        public List<Task> tasks { get; set; } = new List<Task>();
        public bool isCompleted { get; set; } = false;

        public double completionPercentage
        {
            get
            {
                if (tasks.Count == 0)
                {
                    return 0;
                }

                int completedCount = tasks.Count(task => task.isCompleted);

                return ((double)completedCount / tasks.Count) * 100;
            }
        }
    }
}
