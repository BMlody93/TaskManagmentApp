using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Common.CommonModels.TaskModels;

namespace TaskManagmentApp.DataAccess.Interfaces
{
    public interface ITaskDataProvider
    {
        IQueryable<Common.CommonModels.TaskModels.Task> GetTasks();

        System.Threading.Tasks.Task UpdateTasksAsync(IEnumerable<Common.CommonModels.TaskModels.Task> tasks);
    }
}
