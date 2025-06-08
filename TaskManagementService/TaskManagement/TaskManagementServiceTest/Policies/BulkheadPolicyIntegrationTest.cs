using System.Collections.Concurrent;
using Moq;
using TaskManagementServiceRepo.Policies;
using TaskManagementServiceDAO.DTOs;
using TaskManagementServiceRepo.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TaskManagementServiceTests.Policies
{
    [TestClass]
    public class BulkheadPolicyTaskRepoTest
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
        public async Task BulkheadPolicy_LimitsConcurrent_GetAllTasksWithUserNameASync()
        {
            // Arrange
            var mockRepo = new Mock<ITaskRepo>();
            mockRepo.Setup(r => r.GetAllTasksWithUserNameASync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskDTO> { new TaskDTO { Id = 1, Title = "Test", AssignedUserNames = new List<string> { "User1" } } });

            var policy = BulkheadPolicy.CreateBulkheadPolicy(CreateTestConfiguration(), "TestName");
            var results = new ConcurrentBag<string>();
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < 5; i++)
            {
                var taskNumber = i;
                var task = Task.Run(async () =>
                {
                    try
                    {
                        await policy.ExecuteAsync(async () =>
                        {
                            var response = await mockRepo.Object.GetAllTasksWithUserNameASync();
                            results.Add($"Task {taskNumber} succeeded: {response.Count} tasks");
                        });
                    }
                    catch (Exception ex)
                    {
                        results.Add($"Task {taskNumber} rejected: {ex.Message}");
                    }
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            // Assert
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Assert.AreEqual(5, results.Count);
            Assert.IsTrue(results.Count(r => r.Contains("succeeded")) <= 2);
            Assert.IsTrue(results.Count(r => r.Contains("rejected")) >= 3);
        }
    }
}