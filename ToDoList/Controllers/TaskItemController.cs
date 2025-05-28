using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ToDoList.Dto;
using ToDoList.Interfaces;
using ToDoList.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : Controller
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ILogger<TaskItemController> _logger;
        private readonly IMapper _mapper;

        public TaskItemController(ITaskItemRepository taskItemRepository, ILogger<TaskItemController> logger,IMapper mapper)
        {
            _taskItemRepository = taskItemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string? status, [FromQuery] string? priority, [FromQuery] DateTime? dueDate)
        {
            _logger.LogInformation("Fetching tasks with filters: Status={Status}, Priority={Priority}, DueDate={DueDate}", status, priority, dueDate);

            var taskEntities = await _taskItemRepository.GetTasksAsync(status, priority, dueDate);
            var taskDtos = _mapper.Map<List<TaskItemDto>>(taskEntities);

            _logger.LogInformation("Retrieved {Count} task(s)", taskDtos.Count);
            return Ok(new ApiResponse<List<TaskItemDto>>(true, "Tasks retrieved successfully", taskDtos));
        }

        [HttpGet("{id}")]
        
        public IActionResult GetTaskById(int id)
        {
            _logger.LogInformation("Fetching task with ID: {Id}", id);

            if (!_taskItemRepository.TaskExists(id))
            {
                _logger.LogWarning("Task with ID {Id} not found", id);
                return NotFound(new ApiResponse<TaskItem>(false, "Task not found"));
            }

            var task = _mapper.Map<TaskItemDto>(_taskItemRepository.GetTaskById(id));

            return Ok(new ApiResponse<TaskItemDto>(true, "Task retrieved", task));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemDto taskCreate, [FromQuery] string? status, [FromQuery] string? priority, [FromQuery] DateTime? dueDate)
        {
            _logger.LogInformation("Attempting to create a task titled: {Title}", taskCreate?.Title);

            if (taskCreate == null)
            {
                _logger.LogWarning("CreateTask called with null input");
                return BadRequest(new ApiResponse<string>(false, "No data input"));
            }

            var tasks = await _taskItemRepository.GetTasksAsync(status, priority, dueDate);

            var task = tasks
                .Where(t => t.Title.Trim().ToUpper() == taskCreate.Title.Trim().ToUpper())
                .FirstOrDefault();

            if (task != null)
            {
                _logger.LogWarning("Task with title {Title} already exists", taskCreate.Title);
                return BadRequest(new ApiResponse<string>(false, "Task already exists"));
            }

            var taskItemMap = _mapper.Map<TaskItem>(taskCreate);

            var created = await _taskItemRepository.CreateTask(taskItemMap);

            if (!created)
            {
                _logger.LogError("Failed to create task with title {Title}", taskCreate.Title);
                return BadRequest(new ApiResponse<string>(false, "Failed to create task"));
            }

            _logger.LogInformation("Task created: {Title}", taskItemMap.Title);
            return Ok(new ApiResponse<TaskItem>(true, "Task created", taskItemMap));
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemDto updatedTask)
        {
            _logger.LogInformation("Updating task with ID: {Id}", id);

            if (updatedTask == null)
            {
                _logger.LogWarning("UpdateTask called with null input");
                return BadRequest(new ApiResponse<string>(false, "No data input"));
            }


            if (!_taskItemRepository.TaskExists(id))
            {
                _logger.LogWarning("Cannot update task. Task with ID {Id} not found", id);
                return NotFound(new ApiResponse<string>(false, "Task not found"));
            }
            
            var taskItemMap = _mapper.Map<TaskItem>(updatedTask);

            var updated = await _taskItemRepository.UpdateTask(taskItemMap);

            if (!updated)
            {
                _logger.LogError("Failed to update task with ID {Id}", id);
                return BadRequest(new ApiResponse<string>(false, "Failed to update task"));
            }

            _logger.LogInformation("Task updated: {Id}", id);
            return Ok(new ApiResponse<TaskItem>(true, "Task updated", taskItemMap));
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTask(int id)
        {
            _logger.LogInformation("Deleting task with ID: {Id}", id);

            if (!_taskItemRepository.TaskExists(id))
            {
                _logger.LogWarning("Cannot delete. Task with ID {Id} not found", id);
                return NotFound(new ApiResponse<string>(false, "Task not found"));
            }

            var task = _taskItemRepository.GetTaskById(id);

            if (task == null)
            {
                _logger.LogWarning("GetTaskById returned null for ID {Id}", id);
                return NotFound(new ApiResponse<string>(false, "Task not found"));
            }

            var deleted = await _taskItemRepository.DeleteTask(task);

            if (!deleted)
            {
                _logger.LogError("Failed to delete task with ID {Id}", id);
                return BadRequest(new ApiResponse<string>(false, "Failed to delete task"));
            }

            _logger.LogInformation("Task deleted successfully: {Id}", id);
            return Ok(new ApiResponse<TaskItem>(true, "Task deleted", task));
        }

        [HttpPatch("{id}/complete")]

        public async Task<IActionResult> MarkTask(int id)
        {
            _logger.LogInformation("Marking task as complete: ID = {Id}", id);

            if (!_taskItemRepository.TaskExists(id))
            {
                _logger.LogWarning("Cannot mark task. ID {Id} not found", id);
                return NotFound(new ApiResponse<string>(false, "Task not found"));
            }

            var task = _taskItemRepository.GetTaskById(id);

            if (task == null)
            {
                _logger.LogWarning("Task retrieval failed for ID {Id}", id);
                return NotFound(new ApiResponse<string>(false, "Task not found"));
            }

            var marked = await _taskItemRepository.MarkTask(id);

            if (!marked)
            {
                _logger.LogError("Failed to mark task complete for ID {Id}", id);
                return BadRequest(new ApiResponse<string>(false, "Failed to mark task"));
            }

            _logger.LogInformation("Task marked as complete: ID = {Id}", id);
            return Ok(new ApiResponse<TaskItem>(true, "Task marked", task));


        }
    }

}
