using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Common.CommonModels.TaskModels;
using TaskManagmentApp.Common.ApiModels;

namespace TaskManagmentApp.Business.Interfaces
{
    public interface ITaskManager
    {
        Task<PagedData<Common.CommonModels.TaskModels.Task>> GetUnassignedTasksPagedAsync(int pageNumber, int pageSize);

        Task<PagedData<Common.CommonModels.TaskModels.Task>> GetAssignedTasksPagedAsync(int userId, int pageNumber, int pageSize, bool withStatistics);

        System.Threading.Tasks.Task UpdateTasksAssignmentAsync(int userId, List<int> taskIds);
    }
}
