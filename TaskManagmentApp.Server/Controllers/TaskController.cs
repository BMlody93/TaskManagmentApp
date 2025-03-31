using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using TaskManagmentApp.Business.Interfaces;
using TaskManagmentApp.Business.Managers;
using TaskManagmentApp.Common.ApiModels;
using TaskManagmentApp.Common.CommonModels.TaskModels;
using TaskManagmentApp.Common.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagmentApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskManager _taskManager;
        private int _maxPageSize;
        private int _maxSentTaskIds;

        public TaskController(ITaskManager taskManager, IConfiguration configuration)
        {
            Log.Debug("Creating Controler");
            _taskManager = taskManager;

            _maxPageSize = configuration.GetValue<int>("AppSettings:TaskController:MaxPageSize", 10);
            _maxSentTaskIds  = configuration.GetValue<int>("AppSettings:TaskController:MaxSentTaskIds", 10);            
        }

        // GET: api/<TaskController>/5
        /// <summary>
        /// Method to get list of task assigned to specified users sorted by diffculty and paged.
        /// </summary>
        /// <param name="userId">id of user which task list we want to get</param>
        /// <param name="pageIndex">index of page</param>
        /// <param name="pageSize">size of user list</param>
        /// <param name="withStatistics">if true returns statistics for tasks assigned to user</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<PagedData<Common.CommonModels.TaskModels.Task>>> GetTasks(int userId, [FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] bool withStatistics = false)
        {
            try
            {
                if (pageIndex < 0)
                {
                    throw new CustomException(nameof(pageIndex));
                }

                if (pageSize < 1 || pageSize > _maxPageSize)
                {
                    throw new CustomException(nameof(pageSize));
                }

                Log.Information($"userId = {userId}");
                Log.Information($"pageIndex = {pageIndex}");
                Log.Information($"pageSize = {pageSize}");               
                Log.Information($"withStatistics = {withStatistics}");

                Log.Information("Getting database structure");
                var tasks = await _taskManager.GetAssignedTasksPagedAsync(userId, pageIndex, pageSize, withStatistics);

                Log.Debug("Returning database structure");
                return Ok(tasks);
            }
            catch (CustomException ex)
            {
                Log.Error(ex, $"Error [{ex.Message}] when getting assigned tasks");
                return StatusCode(ex.ErrorCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when trying to get assigned tasks");
                return StatusCode(500);
            }
        }

        // GET: api/<TaskController>
        /// <summary>
        /// Method to get list of unassigned tasks sorted by diffculty and paged.
        /// </summary>
        /// <param name="pageIndex">index of page</param>
        /// <param name="pageSize">size of user list</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedData<Common.CommonModels.TaskModels.Task>>> GetTasks([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            try
            {
                if(pageIndex < 0)
                {
                    throw new CustomException(nameof(pageIndex));
                }

                if (pageSize < 1 || pageSize > _maxPageSize)
                {
                    throw new CustomException(nameof(pageSize));
                }
                Log.Information($"pageIndex = {pageIndex}");
                Log.Information($"pageSize = {pageSize}");

                Log.Information("Getting unassigned tasks");
                var tasks = await _taskManager.GetUnassignedTasksPagedAsync(pageIndex, pageSize);

                Log.Debug("Returning unassigned tasks");
                return Ok(tasks);
            }
            catch (CustomException ex)
            {
                Log.Error(ex, $"Error [{ex.Message}] when getting unassigned tasks");
                return StatusCode(ex.ErrorCode, new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when trying to get unassigned tasks");
                return StatusCode(500);
            }
        }

        // PUT api/<TaskController>/5
        /// <summary>
        /// Method to assign tasks to user
        /// </summary>
        /// <param name="userId">id of the user to which we want to assign tasks</param>
        /// <param name="taskIds">list of task ids which we want to assign to user</param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<ActionResult> AssignNewTasks(int userId, [FromBody] List<int> taskIds)
        {
            try
            {
                if (taskIds is null || taskIds.Count == 0 || taskIds.Count > _maxSentTaskIds)
                {
                    throw new CustomException(nameof(taskIds));
                }

                Log.Information($"userId = {userId}");

                Log.Information("Getting database structure");
                await _taskManager.UpdateTasksAssignmentAsync(userId, taskIds);

                Log.Debug("Returning database structure");
                return Ok();
            }
            catch(CustomException ex)
            {
                Log.Error(ex, $"Error [{ex.Message}] when assiging new tasks");
                return StatusCode(ex.ErrorCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when assiging new tasks");
                return StatusCode(500);
            }
        }
    }
}
