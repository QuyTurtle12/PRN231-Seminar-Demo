using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Bulkhead;

namespace TaskManagementServiceRepo.Policies
{
    public class BulkheadPolicy
    {
        public static AsyncBulkheadPolicy CreateBulkheadPolicy(IConfiguration configuration, string policyName)
        {
            var bulkheadSection = configuration.GetSection($"BulkheadPolicies:{policyName}");
            var maxParallel = int.Parse(bulkheadSection["MaxParallelization"] ?? "0");
            var maxQueued = int.Parse(bulkheadSection["MaxQueuingActions"] ?? "0");
            var timeoutSeconds = int.Parse(bulkheadSection["TimeoutInSeconds"] ?? "0");

            return Policy.BulkheadAsync(
                maxParallelization: maxParallel,
                maxQueuingActions: maxQueued,
                onBulkheadRejectedAsync: context =>
                {
                    Console.WriteLine($"Bulkhead [{policyName}] rejected at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}. " +
                                    $"Max parallel: {maxParallel}, " +
                                    $"Max queued: {maxQueued}");
                    return Task.CompletedTask;
                });
        }
    }
}
