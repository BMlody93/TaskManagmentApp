using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagmentApp.Common.ApiModels
{
    public class PagedData<T>
    {
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public Dictionary<string, object>? Statistics { get; set; }

        public required List<T> Items { get; set; }
    }
}
