using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskmanWebApp.Models
{
    public class GroupModel
    {
        public int id { get; set; }
        public IEnumerable<UserModel> users { get; set; }
        public IEnumerable<TaskModel> tasks { get; set; }
    }
}
