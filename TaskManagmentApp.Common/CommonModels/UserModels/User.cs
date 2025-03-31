using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagmentApp.Common.CommonModels.UserModels
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public UserType UserType { get; set; }

        //Here can be put other properties as required
    }
}
