using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskmanWebApp.Models
{
    public enum TaskStatus { Idle, InProgress, Done };
    public class TaskModel
    {
        public int id { get; set; }
        public TaskStatus status { get; set; }
        public string description { get; set; }
    }
}
