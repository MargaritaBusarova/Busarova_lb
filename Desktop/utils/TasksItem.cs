using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.utils
{
    public class TasksItem
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

    }

}
