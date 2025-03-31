using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Common.CommonModels.UserModels;

namespace TaskManagmentApp.DataAccess.Interfaces
{
    public interface IUserDataProvider
    {
        IQueryable<User> GetUsers();
    }
}
