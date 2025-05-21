using ToDoList.Models;

namespace ToDoList.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetTasksAsync(string? status, string? priority, DateTime? dueDate);
        TaskItem GetTaskById(int id);
        bool TaskExists(int id);
        Task<bool> CreateTask(TaskItem task);
        Task<bool> UpdateTask(TaskItem task);
        Task<bool> DeleteTask(TaskItem task);
        Task<bool> MarkTask(int id);
        Task<bool> SaveAsync();

    }
}
