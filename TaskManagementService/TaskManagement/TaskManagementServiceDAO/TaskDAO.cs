using Microsoft.EntityFrameworkCore;
using TaskManagementServiceBO;
using TaskManagementServiceDAO.Interfaces;

namespace TaskManagementServiceDAO
{
    public class TaskDAO : ITaskDAO
    {
        private TaskManagementContext _context;
        
        public TaskDAO(TaskManagementContext context)
        {
            _context = new TaskManagementContext();
        }

        public async Task<List<TaskManagementServiceBO.Task>> GetAllTasks(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(t => t.Status)
                .Include(t => t.Category)
                .Include(t => t.TaskAssignments)
                    .ThenInclude(ta => ta.User)
                .ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task<int> AddTask(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return task.Id;
        }
        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int taskId, CancellationToken cancellationToken = default)
        {
            var task = await _context.Tasks.FindAsync(taskId, cancellationToken);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
