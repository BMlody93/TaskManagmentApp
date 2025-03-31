using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Common.CommonModels.UserModels;

namespace TaskManagmentApp.Business.Interfaces
{
    public interface IUserManager
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(int userId);
    }
}
