using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskmanWebApp.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public UserModel()
        {

        }
        public UserModel(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
