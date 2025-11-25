using Kanban_Board.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban_Board.GUI
{
    internal class InputHelper
    {
        public static DateTime GetDeadlineFromUser()
        {
            const string format = "DD-MM-YYYY";
            string deadlineInput;

            Console.WriteLine("1: Today");
            Console.WriteLine("2: In a Week");
            Console.WriteLine("3: In a Fortnight");
            Console.WriteLine("4: In a Month");
            Console.WriteLine("5: Custom Date");

            string? input = Console.ReadLine();
            if (int.TryParse(input, out int response))
            {
                switch (response)
                {
                    case 1:
                        deadlineInput = DateTime.Now.ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 2:
                        deadlineInput = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 3:
                        deadlineInput = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 4:
                        deadlineInput = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    case 5:
                        Console.WriteLine($"Please enter a deadline ({format}):");
                        deadlineInput = Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd");
                        return DateTime.Parse(deadlineInput);
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
            return DateTime.Now;
        }

        public static Priority GetPriorityFromUser()
        {
            Priority Priority;
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. High");
            string? userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int Out))
            {
                switch (Out)
                {
                    case 1:
                        Priority = Priority.Low;
                        break;
                    case 2:
                        Priority = Priority.Medium;
                        break;
                    case 3:
                        Priority = Priority.High;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Defaulting to Low priority.");
                        Priority = Priority.Low;
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Defaulting to Low priority.");
                Priority = Priority.Low;
            }
            return Priority;
        }

        public static Status GetStatusFromUser()
        {
            Status Status;
            Console.WriteLine("1. To Do");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Done");
            string? userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int Out))
            {
                switch (Out)
                {
                    case 1:
                        Status = Status.ToDo;
                        break;
                    case 2:
                        Status = Status.InProgress;
                        break;
                    case 3:
                        Status = Status.Done;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Defaulting to To Do status.");
                        Status = Status.ToDo;
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Defaulting to To Do status.");
                Status = Status.ToDo;
            }
            return Status;
        }
    }
}
