using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using TaskManagementServiceRepo.Policies;

namespace TaskManagementServiceTests.Policies
{
    [TestClass]
    public class BulkheadPolicyTest
    {
        private IConfiguration CreateTestConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?>
        {
            {"BulkheadPolicies:TestName:MaxParallelization", "1"},
            {"BulkheadPolicies:TestName:MaxQueuingActions", "1"},
            {"BulkheadPolicies:TestName:TimeoutInSeconds", "10"}
        };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [TestMethod]
        public async Task DemonstrateBulkheadRejection()
        {
            // Arrange
            var policy = BulkheadPolicy.CreateBulkheadPolicy(CreateTestConfiguration(), "TestName");
            var results = new ConcurrentBag<string>();
            var tasks = new List<Task>();

            // Act
            // Create 5 concurrent tasks (more than our limit of 1 executing + 1 queued)
            for (int i = 0; i < 5; i++)
            {
                var taskNumber = i;
                var task = Task.Run(async () =>
                {
                    try
                    {
                        await policy.ExecuteAsync(async () =>
                        {
                            Console.WriteLine($"Task {taskNumber} started");
                            await Task.Delay(2000); // Simulate work taking 2 seconds
                            Console.WriteLine($"Task {taskNumber} completed");
                            results.Add($"Task {taskNumber} succeeded");
                        });
                    }
                    catch (Exception ex)
                    {
                        results.Add($"Task {taskNumber} rejected: {ex.Message}");
                    }
                });
                tasks.Add(task);
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // Assert
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            // We expect:
            // - 1 task to execute immediately
            // - 1 task to be queued
            // - 3 tasks to be rejected
            Assert.AreEqual(5, results.Count);
            Assert.IsTrue(results.Count(r => r.Contains("succeeded")) <= 2);
            Assert.IsTrue(results.Count(r => r.Contains("rejected")) >= 3);
        }
    }
}