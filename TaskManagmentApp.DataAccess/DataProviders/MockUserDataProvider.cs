using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagmentApp.Common.CommonModels.UserModels;
using TaskManagmentApp.DataAccess.Interfaces;
using static System.Reflection.Metadata.BlobBuilder;

namespace TaskManagmentApp.DataAccess.DataProviders
{
    public class MockUserDataProvider : IUserDataProvider
    {
        private List<User> _users;
        public MockUserDataProvider()
        {
            _users = new List<User>
        {
            new User { Id = 1, Username = "john_doe", UserType = UserType.Admin },
            new User { Id = 2, Username = "jane_smith", UserType = UserType.Admin },
            new User { Id = 3, Username = "mike_jones", UserType = UserType.Programmer },
            new User { Id = 4, Username = "anna_brown", UserType = UserType.Programmer },
            new User { Id = 5, Username = "chris_white", UserType = UserType.Programmer }
        };
        }
        public IQueryable<User> GetUsers()
        {
            return _users.AsQueryable<User>();
        }
    }
}
