using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Business.Interfaces;
using TaskManagmentApp.Common.ApiModels;
using TaskManagmentApp.Common.CommonModels.TaskModels;
using TaskManagmentApp.Common.CommonModels.UserModels;
using TaskManagmentApp.Common.Exceptions;
using TaskManagmentApp.DataAccess.Interfaces;

namespace TaskManagmentApp.Business.Managers
{
    public class TaskManager : ITaskManager
    {
        private readonly ITaskDataProvider _taskDataProvider;
        private readonly IUserDataProvider _userDataProvider;

        public TaskManager(ITaskDataProvider taskDataProvider, IUserDataProvider userDataProvider)
        {
            _taskDataProvider = taskDataProvider ?? throw new ArgumentNullException(nameof(taskDataProvider));
            _userDataProvider = userDataProvider ?? throw new ArgumentNullException(nameof(userDataProvider));
        }

        public async Task<PagedData<Common.CommonModels.TaskModels.Task>> GetAssignedTasksPagedAsync(int userId, int pageIndex, int pageSize, bool withStatistics)
        {
            try
            {
                if (pageIndex < 0)
                {
                    throw new CustomException(nameof(pageIndex));
                }

                if (pageSize < 1)
                {
                    throw new CustomException(nameof(pageSize));
                }

                Log.Information($"userId = {userId}");
                Log.Information($"pageIndex = {pageIndex}");
                Log.Information($"pageSize = {pageSize}");
                Log.Information($"withStatistics = {withStatistics}");

                var pagedTasks = new PagedData<Common.CommonModels.TaskModels.Task>()
                {
                    Items = await _taskDataProvider.GetTasks()
                        .Where(j => j.AssignedUserId == userId)
                        .OrderByDescending(j => j.Difficulty)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToListAsync(),
                    TotalItems = _taskDataProvider.GetTasks()
                        .Where(j => j.AssignedUserId == userId)
                        .Count(),
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Statistics = new Dictionary<string, object>()
                };

                if (withStatistics)
                {
                    pagedTasks.Statistics.Add("HardTasksCount", _taskDataProvider.GetTasks()
                        .Where(j => j.AssignedUserId == userId && (j.Difficulty == 4 || j.Difficulty == 5)).Count());
                    pagedTasks.Statistics.Add("EasyTasksCount", _taskDataProvider.GetTasks()
                        .Where(j => j.AssignedUserId == userId && (j.Difficulty == 1 || j.Difficulty == 2)).Count());
                    pagedTasks.Statistics.Add("TotalTasksCount", pagedTasks.TotalItems);
                }

                return pagedTasks;
            }
            catch (Exception ex) {
                Log.Error(ex, "Error when getting assigned tasks");
                throw;
            }
        }

        public async Task<PagedData<Common.CommonModels.TaskModels.Task>> GetUnassignedTasksPagedAsync(int pageIndex, int pageSize)
        {
            try
            {
                if (pageIndex < 0)
                {
                    throw new CustomException(nameof(pageIndex));
                }

                if (pageSize < 1)
                {
                    throw new CustomException(nameof(pageSize));
                }

                Log.Information($"pageIndex = {pageIndex}");
                Log.Information($"pageSize = {pageSize}");

                var pagedTasks = new PagedData<Common.CommonModels.TaskModels.Task>()
                {
                    Items = await _taskDataProvider.GetTasks()
                        .Where(j => j.AssignedUserId == null)
                        .OrderByDescending(j => j.Difficulty)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToListAsync(),
                    TotalItems = _taskDataProvider.GetTasks()
                        .Where(j => j.AssignedUserId == null)
                        .Count(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedTasks;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when getting unassigned tasks");
                throw;
            }
        }

        public async System.Threading.Tasks.Task UpdateTasksAssignmentAsync(int userId, List<int> taskIds)
        {
            try
            {
                if (taskIds is null || taskIds.Count == 0)
                {
                    throw new CustomException(nameof(taskIds));
                }

                Log.Information($"userId = {userId}");
                Log.Information($"newTaskIds [{string.Join(",", taskIds)}]");

                //Check if user exist
                if (userId <= 0)
                {
                    throw new CustomException(nameof(userId));
                }

                var user = _userDataProvider.GetUsers().FirstOrDefault(u => u.Id == userId);

                if (user is null)
                {
                    throw new CustomException($"User with id {userId} does not exist");
                }

                Log.Information($"Getting tasks currently assigned to user with id {userId} from db");
                //get list of curently assaigned tasks to user
                var assignedTasks = await _taskDataProvider.GetTasks()
                    .Where(j => j.AssignedUserId == userId).ToListAsync();

                Log.Information($"Getting tasks with id in list [{string.Join(",", taskIds)}] from db");
                //get list of new tasks
                var newTasks = await _taskDataProvider.GetTasks()
                    .Where(j => taskIds.Contains(j.Id)).ToListAsync();

                //check if all new tasks exist
                var notFoundTasks = taskIds.Except(newTasks.Select(j => j.Id));
                if (notFoundTasks.Any())
                {
                    throw new Exception("One of sent task ids does not have matching task");
                }

                //check if all new tasks are unasigned
                var alreadyAssignedTasks = newTasks.Where(j => j.AssignedUserId is not null);
                if (alreadyAssignedTasks.Any())
                {
                    throw new Exception("One of sent task id is already assigned to user");
                }

                assignedTasks.AddRange(newTasks);

                if(assignedTasks.Count() < 5)
                {
                    throw new Exception("User wont have enogh tasks. Number of task should be between 5 and 11");
                }

                //check if number of tasks will be between 5 and 11
                if (assignedTasks.Count > 11)
                {
                    throw new Exception("User will have too many tasks. Number of task should be between 5 and 11");
                }

                //check if number of hard tasks is in the allowed bounds
                var hardTasksCount = assignedTasks.Where(j => j.Difficulty == 4 || j.Difficulty == 5).Count();
                var hardTasksPercentage = ((double)hardTasksCount / (double)assignedTasks.Count) * 100;
                if (hardTasksPercentage > 30)
                {
                    throw new Exception("Too many hard task. Tasks with difficulty 4 and 5 should constitute between 10 and 30 percentage of all tasks.");
                }
                if (hardTasksPercentage < 10)
                {
                    throw new Exception("Not enough hard task. Tasks with difficulty 4 and 5 should constitute between 10 and 30 percentage of all tasks.");
                }

                //check if number of easy tasks is in the allowed bounds
                var easyTasksCount = assignedTasks.Where(j => j.Difficulty == 1 || j.Difficulty == 2).Count();
                var easyTasksPercentage = ((double)easyTasksCount / (double)assignedTasks.Count) * 100;
                if (easyTasksPercentage > 50)
                {
                    throw new Exception("Too many easy task. Tasks with difficulty 1 and 2 can 't constitute more than 50 percentage of all tasks.");
                }

                //check if user of type programer has only tasks of type TaskType.Implementation
                if (user.UserType == UserType.Programmer
                    && assignedTasks.Any(j => j.TaskType != TaskType.Implementation))
                {
                    throw new Exception("User of type Programmer cannot have tasks of type other than Implementation");
                }

                //update all tasks
                assignedTasks.ForEach(j => { j.AssignedUserId = user.Id; });

                Log.Information("Assignin tasks to user in db");
                await _taskDataProvider.UpdateTasksAsync(assignedTasks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when assigning tasks");
                throw;
            }
        }
    }
}
