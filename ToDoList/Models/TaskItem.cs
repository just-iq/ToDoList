using static ToDoList.Enums.TaskEnums;

namespace ToDoList.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description  { get; set; }
        public DateTime DueDate { get; set; }
        public StatusTask Status { get; set; }
        public PriorityTask Priority { get; set; }
    }
}
