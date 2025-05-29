using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ToDoList.Dto;
using ToDoList.Interfaces;
using ToDoList.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static ToDoList.Enums.TaskEnums;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskItemController> _logger;

        public TaskItemController(ITaskService taskService, ILogger<TaskItemController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] StatusTask? status, [FromQuery] PriorityTask? priority, [FromQuery] DateTime? dueDate)
        {
            _logger.LogInformation("Fetching tasks");
            var response = await _taskService.GetTasksAsync(status, priority, dueDate);
            return StatusCode(response.Success ? 200 : 400, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            _logger.LogInformation("Fetching task {Id}", id);
            var response = await _taskService.GetTaskByIdAsync(id);
            return StatusCode(response.Success ? 200 : 404, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemDto dto, [FromQuery] StatusTask? status, [FromQuery] PriorityTask? priority, [FromQuery] DateTime? dueDate)
        {
            _logger.LogInformation("Creating task");
            var response = await _taskService.CreateTaskAsync(dto, status, priority, dueDate);
            return StatusCode(response.Success ? 200 : 400, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemDto dto)
        {
            _logger.LogInformation("Updating task");
            var response = await _taskService.UpdateTaskAsync(id, dto);
            return StatusCode(response.Success ? 200 : 400, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            _logger.LogInformation("Deleting task");
            var response = await _taskService.DeleteTaskAsync(id);
            return StatusCode(response.Success ? 200 : 404, response);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkComplete(int id)
        {
            _logger.LogInformation("Marking task complete");
            var response = await _taskService.MarkTaskCompleteAsync(id);
            return StatusCode(response.Success ? 200 : 400, response);
        }
    }

}
