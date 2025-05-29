using AutoMapper;
using ToDoList.Dto;
using ToDoList.Interfaces;
using ToDoList.Models;
using static ToDoList.Enums.TaskEnums;

namespace ToDoList.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskItemRepository _repository;
        private readonly IMapper _mapper;

        public TaskService(ITaskItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<TaskItem>>> GetTasksAsync(StatusTask? status, PriorityTask? priority, DateTime? dueDate)
        {
            var tasks = await _repository.GetTasksAsync(status, priority, dueDate);
            return new ApiResponse<List<TaskItem>>(true, "Tasks retrieved", tasks);
        }

        public async Task<ApiResponse<TaskItem>> GetTaskByIdAsync(int id)
        {
            if (!_repository.TaskExists(id))
                return new ApiResponse<TaskItem>(false, "Task not found");

            var task = _repository.GetTaskById(id);
            return new ApiResponse<TaskItem>(true, "Task found", task);
        }

        public async Task<ApiResponse<TaskItem>> CreateTaskAsync(TaskItemDto taskDto, StatusTask? status, PriorityTask? priority, DateTime? dueDate)
        {
            var tasks = await _repository.GetTasksAsync(status, priority, dueDate);
            if (tasks.Any(t => t.Title.Trim().ToUpper() == taskDto.Title.Trim().ToUpper()))
                return new ApiResponse<TaskItem>(false, "Task already exists");

            var task = _mapper.Map<TaskItem>(taskDto);
            task.Status = StatusTask.Pending;

            var success = await _repository.CreateTask(task);

            return success
                ? new ApiResponse<TaskItem>(true, "Task created", task)
                : new ApiResponse<TaskItem>(false, "Failed to create task");
        }

        public async Task<ApiResponse<TaskItem>> UpdateTaskAsync(int id, TaskItemDto taskDto)
        {
            if (!_repository.TaskExists(id))
                return new ApiResponse<TaskItem>(false, "Task not found");

            var task = _mapper.Map<TaskItem>(taskDto);
            task.Id = id;

            var success = await _repository.UpdateTask(task);

            return success
                ? new ApiResponse<TaskItem>(true, "Task updated", task)
                : new ApiResponse<TaskItem>(false, "Failed to update task");
        }

        public async Task<ApiResponse<TaskItem>> DeleteTaskAsync(int id)
        {
            if (!_repository.TaskExists(id))
                return new ApiResponse<TaskItem>(false, "Task not found");

            var task = _repository.GetTaskById(id);
            var success = await _repository.DeleteTask(task);

            return success
                ? new ApiResponse<TaskItem>(true, "Task deleted", task)
                : new ApiResponse<TaskItem>(false, "Failed to delete task");
        }

        public async Task<ApiResponse<TaskItem>> MarkTaskCompleteAsync(int id)
        {
            if (!_repository.TaskExists(id))
                return new ApiResponse<TaskItem>(false, "Task not found");

            var task = _repository.GetTaskById(id);
            var success = await _repository.MarkTask(id);

            return success
                ? new ApiResponse<TaskItem>(true, "Task marked complete", task)
                : new ApiResponse<TaskItem>(false, "Failed to mark task");
        }
    }
}
