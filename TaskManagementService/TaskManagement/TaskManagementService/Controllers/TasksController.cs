using Microsoft.AspNetCore.Mvc;
using Polly.Bulkhead;
using TaskManagementServiceRepo.Interfaces;

namespace TaskManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepo _taskRepo;
        

        public TasksController(ITaskRepo taskRepo)
        {
            _taskRepo = taskRepo;   
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksWithUserName(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _taskRepo.GetAllTasksWithUserNameASync(cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
