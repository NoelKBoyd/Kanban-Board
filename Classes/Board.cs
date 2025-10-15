namespace Kanban_Board.Classes
{
    internal class Board
    {
        public required string name { get; set; }
        public string? description { get; set; }
        public bool isCompleted { get; set; } = false;
        public List<List> lists { get; set; } = new List<List>();

        public double completionPercentage
        {
            get
            {
                if (lists.Count == 0)
                {
                    return 0;
                }

                int completedCount = lists.Count(list => list.isCompleted);

                return ((double)completedCount / lists.Count) * 100;
            }
        }
    }
}
