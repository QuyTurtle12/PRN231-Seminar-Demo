using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Bulkhead;

namespace TaskManagementServiceRepo.Policies
{
    public class BulkheadPolicy
    {
        public static AsyncBulkheadPolicy CreateBulkheadPolicy(IConfiguration configuration)
        {
            // Get values from configuration with fallbacks
            var maxParallelTasks = configuration.GetValue<int>("BulkheadPolicy:MaxParallelization", 3);
            var maxQueuedTasks = configuration.GetValue<int>("BulkheadPolicy:MaxQueuingActions", 5);
            var timeoutSeconds = configuration.GetValue<int>("BulkheadPolicy:TimeoutInSeconds", 30);

            return Policy.BulkheadAsync(
                maxParallelization: maxParallelTasks,
                maxQueuingActions: maxQueuedTasks,
                onBulkheadRejectedAsync: context =>
                {
                    Console.WriteLine($"Bulkhead rejected at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}. " +
                                    $"Max parallel tasks: {maxParallelTasks}, " +
                                    $"Max queued tasks: {maxQueuedTasks}");
                    return Task.CompletedTask;
                });
        }
    }
}
