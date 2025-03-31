using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TaskManagmentApp.Business.Interfaces;
using TaskManagmentApp.Common.CommonModels.UserModels;
using TaskManagmentApp.Common.Exceptions;

namespace TaskManagmentApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager) {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            try
            {
                Log.Information("Getting users");
                var users = await _userManager.GetUsersAsync();

                Log.Debug("Returning users");
                return Ok(users);
            }
            catch (CustomException ex)
            {
                Log.Error(ex, $"Error [{ex.Message}] when getting users");
                return StatusCode(ex.ErrorCode, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when trying to get getting users");
                return StatusCode(500);
            }

        }
    }
}
