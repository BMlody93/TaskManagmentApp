using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagmentApp.Common.CommonModels.TaskModels
{
    public class Task
    {
        public int Id { get; set; }
        public int? AssignedUserId { get; set; }
        public required string Title { get; set; }
        public required string Text { get; set; }
        public int Difficulty { get; set; }
        public TaskType TaskType { get; set; }
        public required Dictionary<string, object> Details {get;set;}
    }
}
