using static ToDoList.Enums.TaskEnums;

namespace ToDoList.Dto
{
    public class TaskItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityTask Priority { get; set; }
    }
}
