using ToDoList.Models;

namespace ToDoList.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetTasksAsync(string? status, string? priority, DateTime? dueDate);
        TaskItem GetTaskById(int id);
        bool TaskExists(int id);
        bool CreateTask(TaskItem task);
        bool UpdateTask(TaskItem task);
        bool DeleteTask(TaskItem task);
        bool MarkTask(int id);
        bool Save();

    }
}
