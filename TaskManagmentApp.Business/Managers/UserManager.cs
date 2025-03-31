using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Business.Interfaces;
using TaskManagmentApp.Common.CommonModels.UserModels;
using TaskManagmentApp.DataAccess.Interfaces;

namespace TaskManagmentApp.Business.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserDataProvider _userDataProvider;

        public UserManager(IUserDataProvider userDataProvider) {
            _userDataProvider = userDataProvider ?? throw new ArgumentNullException(nameof(userDataProvider));
        }
        public async Task<User> GetUserAsync(int userId)
        {
            return await _userDataProvider.GetUsers().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _userDataProvider.GetUsers().ToListAsync();
        }
    }
}
