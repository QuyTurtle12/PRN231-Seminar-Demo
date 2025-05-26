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
        private readonly AsyncBulkheadPolicy _bulkheadPolicy;

        public TasksController(ITaskRepo taskRepo, AsyncBulkheadPolicy bulkheadPolicy)
        {
            _taskRepo = taskRepo;   
            _bulkheadPolicy = bulkheadPolicy;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksWithUserName(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _bulkheadPolicy.ExecuteAsync(
                    async ct => await _taskRepo.GetAllTasksWithUserNameASync(ct),
                    cancellationToken);

                return Ok(result);
            }
            catch (BulkheadRejectedException)
            {
                return StatusCode(429, "Bulkhead rejected: Too many concurrent requests.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
