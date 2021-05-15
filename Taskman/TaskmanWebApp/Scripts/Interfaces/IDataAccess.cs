using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskmanWebApp.Models;

namespace TaskmanWebApp.Scripts.Interfaces
{
    public interface IDataAccess
    {
        public Task<UserModel> GetUserAsync(int uid);
        public Task<UserModel> GetUserAsync(string username);
        public Task<bool> CreateUserAsync(string username, string password);

        public Task<GroupModel> GetGroupAsync(int id);
        public Task<GroupModel> GetGroupAsync(string name);
        public Task<bool> CreateGroupAsync(string name);
        public Task<bool> AssignUserToGroupAsync(int uid, int gid);

        public Task<TaskModel> GetTaskAsync(int id);
        public Task<bool> CreateTaskAsync(string description, int gid);
        public Task<bool> UpdateTaskStatusAsync(int tid, TaskmanWebApp.Models.TaskStatus newStatus);
    }
}
