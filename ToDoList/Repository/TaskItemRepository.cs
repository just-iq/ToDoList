using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly DataContext _context;

        public TaskItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTask(TaskItem task)
        {
            _context.Add(task);
            return await SaveAsync();
        }

        public async Task<bool> DeleteTask(TaskItem task)
        {
            _context.Remove(task);
            return await SaveAsync();
        }

        public TaskItem GetTaskById(int id)
        {
            return _context.Tasks.Where(t => t.Id == id).FirstOrDefault();
        }

        public async Task<List<TaskItem>> GetTasksAsync(string? status, string? priority, DateTime? dueDate)
        {
            var query = _context.Tasks.AsQueryable();

            if (status != null)
            {
                query = query.Where(t => t.status == status);
            }

            if (priority != null)
            {
                query = query.Where(t => t.Priority == priority);
            }

            if (dueDate != null)
            {
                query = query.Where(t => t.DueDate.Date == dueDate.Value.Date);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> MarkTask(int id)
        {
            var foundTask = _context.Tasks.Where(t => t.Id == id).FirstOrDefault();

            if (foundTask == null)
            {
                return false;
            }

            foundTask.status = "Completed";

            return await SaveAsync();

        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }


        public bool TaskExists(int id)
        {
            return _context.Tasks.Any(t => t.Id == id);
        }

        public async Task<bool> UpdateTask(TaskItem task)
        {
            _context.Update(task);
            return await SaveAsync();
        }
    }
}
