using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Common.CommonModels.TaskModels;
using TaskManagmentApp.DataAccess.Interfaces;

namespace TaskManagmentApp.DataAccess.DataProviders
{
    public class MockTaskDataProvider : ITaskDataProvider
    {
        private List<Common.CommonModels.TaskModels.Task> _tasks;
        public MockTaskDataProvider()
        {
            _tasks = GenerateMockTasks();
        }

        public IQueryable<Common.CommonModels.TaskModels.Task> GetTasks()
        {
            return _tasks.AsQueryable<Common.CommonModels.TaskModels.Task>();
        }

        public async System.Threading.Tasks.Task UpdateTasksAsync(IEnumerable<Common.CommonModels.TaskModels.Task> tasks)
        {
            // Simulate async behavior (you'd do real DB operations in a real implementation)
            await System.Threading.Tasks.Task.CompletedTask;
        }

        private static Random _random = new Random();
        private static List<Common.CommonModels.TaskModels.Task> GenerateMockTasks()
        {
            var tasks = new List<Common.CommonModels.TaskModels.Task>();
            tasks.Add(GenerateMockTask(1, 1, 1, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(2, 1, 2, TaskType.Implementation, Status.Finished));
            tasks.Add(GenerateMockTask(3, 1, 3, TaskType.Implementation, Status.Finished));
            tasks.Add(GenerateMockTask(4, 1, 4, TaskType.Implementation, Status.Finished));
            tasks.Add(GenerateMockTask(5, 1, 5, TaskType.Deployment, Status.Finished));

            tasks.Add(GenerateMockTask(6, 3, 1, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(7, 3, 2, TaskType.Implementation, Status.Finished));
            tasks.Add(GenerateMockTask(8, 3, 3, TaskType.Implementation, Status.Finished));
            tasks.Add(GenerateMockTask(9, 3, 4, TaskType.Implementation, Status.Finished));
            tasks.Add(GenerateMockTask(10, 3, 5, TaskType.Implementation, Status.Finished));

            tasks.Add(GenerateMockTask(11, null, 1, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(12, null, 2, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(13, null, 3, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(14, null, 4, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(15, null, 5, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(16, null, 1, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(17, null, 2, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(18, null, 3, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(19, null, 4, TaskType.Implementation, Status.Unfinished));
            tasks.Add(GenerateMockTask(20, null, 5, TaskType.Implementation, Status.Unfinished));

            tasks.Add(GenerateMockTask(21, null, 1, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(22, null, 2, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(23, null, 3, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(24, null, 4, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(25, null, 5, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(26, null, 1, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(27, null, 2, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(28, null, 3, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(29, null, 4, TaskType.Deployment, Status.Unfinished));
            tasks.Add(GenerateMockTask(30, null, 5, TaskType.Deployment, Status.Unfinished));

            tasks.Add(GenerateMockTask(31, null, 1, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(32, null, 2, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(33, null, 3, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(34, null, 4, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(35, null, 5, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(36, null, 1, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(37, null, 2, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(38, null, 3, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(39, null, 4, TaskType.Maintenance, Status.Unfinished));
            tasks.Add(GenerateMockTask(40, null, 5, TaskType.Maintenance, Status.Unfinished));
            return tasks;
        }

        private static Common.CommonModels.TaskModels.Task GenerateMockTask(int taskId, int? assignedUser,
            int difficulty, TaskType taskType, Status status)
        {

            var task = new Common.CommonModels.TaskModels.Task
            {
                Id = taskId,
                AssignedUserId = assignedUser,
                Title = $"Task {taskId}",
                Text = $"Task {taskId} Description",
                Difficulty = difficulty,
                TaskType = taskType,
                Details = new Dictionary<string, object>()
            };
            switch (taskType)
            {
                case TaskType.Maintenance:
                    task.Details.Add("Deadline", DateTime.Now.AddDays(_random.Next(1, 30)));
                    task.Details.Add("Status", status);
                    task.Details.Add("Text", $"List of servers for task {taskId}");
                    
                    break;
                case TaskType.Deployment:
                    task.Details.Add("Deadline", DateTime.Now.AddDays(_random.Next(1, 30)));
                    task.Details.Add("Status", status);
                    task.Details.Add("Text", $"Scope of implementation for task {taskId}");
                    break;
                case TaskType.Implementation:
                    task.Details.Add("Status", status);
                    task.Details.Add("Text", $"Content of the task {taskId}");
                    break;
            }


            return task;
        }

    }
}
