using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly.Bulkhead;
using TaskManagementServiceRepo.Interfaces;

namespace TaskManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly AsyncBulkheadPolicy _bulkheadPolicy;

        public UsersController(IUserRepo userRepo, [FromKeyedServices("UsersBulkhead")] AsyncBulkheadPolicy bulkheadPolicy)
        {
            _userRepo = userRepo;
            _bulkheadPolicy = bulkheadPolicy;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _bulkheadPolicy.ExecuteAsync(
                    async ct => await _userRepo.GetAllUsers(ct),
                    cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
