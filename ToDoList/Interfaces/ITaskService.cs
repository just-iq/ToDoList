using ToDoList.Dto;
using ToDoList.Models;
using static ToDoList.Enums.TaskEnums;

namespace ToDoList.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<List<TaskItem>>> GetTasksAsync(StatusTask? status, PriorityTask? priority, DateTime? dueDate);
        Task<ApiResponse<TaskItem>> GetTaskByIdAsync(int id);
        Task<ApiResponse<TaskItem>> CreateTaskAsync(TaskItemDto taskDto, StatusTask? status, PriorityTask? priority, DateTime? dueDate);
        Task<ApiResponse<TaskItem>> UpdateTaskAsync(int id, TaskItemDto taskDto);
        Task<ApiResponse<TaskItem>> DeleteTaskAsync(int id);
        Task<ApiResponse<TaskItem>> MarkTaskCompleteAsync(int id);
    }
}
