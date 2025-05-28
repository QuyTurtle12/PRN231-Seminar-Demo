using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Bulkhead;

namespace TaskManagementServiceRepo.Policies
{
    public class BulkheadPolicy
    {
        public static AsyncBulkheadPolicy CreateBulkheadPolicy(IConfiguration configuration, string policyName)
        {
            var maxParallel = configuration.GetValue<int>($"BulkheadPolicies:{policyName}:MaxParallelization");
            var maxQueued = configuration.GetValue<int>($"BulkheadPolicies:{policyName}:MaxQueuingActions");
            var timeoutSeconds = configuration.GetValue<int>($"BulkheadPolicies:{policyName}:TimeoutInSeconds");

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
