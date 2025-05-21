using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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
        private readonly IMapper _mapper;

        public TaskItemController(ITaskItemRepository taskItemRepository, IMapper mapper)
        {
            _taskItemRepository = taskItemRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string? status, [FromQuery] string? priority, [FromQuery] DateTime? dueDate)
        {
            var tasks = await _taskItemRepository.GetTasksAsync(status, priority, dueDate);
            return Ok(new ApiResponse<List<TaskItem>>(true, "Tasks retrieved successfully", tasks));
        }

        [HttpGet("{id}")]
        
        public IActionResult GetTaskById(int id)
        {
            if (!_taskItemRepository.TaskExists(id))
            {
                return NotFound(new ApiResponse<TaskItem>(false, "Task not found"));
            }

            var task = _taskItemRepository.GetTaskById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem taskCreate, [FromQuery] string? status, [FromQuery] string? priority, [FromQuery] DateTime? dueDate)
        {
            if (taskCreate == null)
                return BadRequest(new ApiResponse<string>(false, "No data input"));

            var tasks = await _taskItemRepository.GetTasksAsync(status, priority, dueDate);

            var task = tasks
                .Where(t => t.Title.Trim().ToUpper() == taskCreate.Title.Trim().ToUpper())
                .FirstOrDefault();

            if (task != null)
                return BadRequest(new ApiResponse<string>(false, "Task already exists"));

            var created = await _taskItemRepository.CreateTask(taskCreate);

            if (!created)
                return BadRequest(new ApiResponse<string>(false, "Failed to create task"));

            return Ok(new ApiResponse<TaskItem>(true, "Task created", taskCreate));
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            if (updatedTask == null)
                return BadRequest(new ApiResponse<string>(false, "No data input"));

            if (id != updatedTask.Id)
                return BadRequest(new ApiResponse<string>(false, "Task ID mismatch"));

            if (!_taskItemRepository.TaskExists(id))
                return NotFound(new ApiResponse<string>(false, "Task not found"));

            var task = _taskItemRepository.GetTaskById(id);

            var updated = await _taskItemRepository.UpdateTask(updatedTask);

            if (!updated)
                return BadRequest(new ApiResponse<string>(false, "Failed to update task"));

            return Ok(new ApiResponse<TaskItem>(true, "Task updated", updatedTask));
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTask(int id)
        {
            if (!_taskItemRepository.TaskExists(id))
                return NotFound(new ApiResponse<string>(false, "Task not found"));

            var task = _taskItemRepository.GetTaskById(id);

            if (task == null)
                return NotFound(new ApiResponse<string>(false, "Task not found"));

            var deleted = await _taskItemRepository.DeleteTask(task);

            if (!deleted)
                return BadRequest(new ApiResponse<string>(false, "Failed to delete task"));

            return Ok(new ApiResponse<TaskItem>(true, "Task deleted", task));
        }

        [HttpPatch("{id}/complete")]

        public async Task<IActionResult> MarkTask(int id)
        {
            if (!_taskItemRepository.TaskExists(id))
                return NotFound(new ApiResponse<string>(false, "Task not found"));

            var task = _taskItemRepository.GetTaskById(id);

            if (task == null)
                return NotFound(new ApiResponse<string>(false, "Task not found"));

            var marked = await _taskItemRepository.MarkTask(id);

            if (!marked)
                return BadRequest(new ApiResponse<string>(false, "Failed to mark task"));
            return Ok(new ApiResponse<TaskItem>(true, "Task marked", task));


        }
    }

}
