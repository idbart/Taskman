using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskmanWebApp.Models;
using TaskmanWebApp.Scripts.Interfaces;
using System.Data.SQLite;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace TaskmanWebApp.Scripts
{
    // data access using Sqlite
    // class used as a scoped service so db connection is handled in constructor
    // other methods are self explanitory

    public class LocalDataAccess : IDataAccess
    {
        private IDbConnection _connection;

        public LocalDataAccess(IConfiguration config)
        {
            try
            {
                // so this is not working, do somthing about it
                _connection = new SQLiteConnection(config.GetConnectionString("Local"));
                _connection.Open();
            }
            catch(Exception exe)
            {
                Console.WriteLine(exe.ToString());
                _connection = null;
            }
        }
        ~LocalDataAccess()
        {
            _connection.Close();
        }

        public async Task<GroupModel> GetGroupAsync(int id)
        {
            // check if gid is > 1 because 1 is the id of the default group and there is no group 0
            if (id > 1)
            {
                string query = "SELECT * FROM groups WHERE id=@id";
                object paramsObj = new { id };

                GroupModel group = await _connection.QueryFirstAsync<GroupModel>(query, paramsObj);

                query = "SELECT (id, username, gid) FROM users WHERE gid=@id";
                group.users = await _connection.QueryAsync<UserModel>(query, paramsObj);

                query = "SELECT * FROM tasks WHERE gid=@id";
                group.tasks = await _connection.QueryAsync<TaskModel>(query, paramsObj);

                return group;
            }
            else
            {
                return null;
            }
        }

        public Task<GroupModel> GetGroupAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<TaskModel> GetTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetUserAsync(int uid)
        {
            string query = "SELECT * FROM users WHERE id=@uid";
            object paramsObj = new { uid };

            return await _connection.QueryFirstAsync<UserModel>(query, paramsObj);
        }

        public async Task<UserModel> GetUserAsync(string username)
        {
            string query = "SELECT * FROM users WHERE username=@username";
            object paramsObj = new { username };

            try
            {
                return await _connection.QueryFirstAsync<UserModel>(query, paramsObj);
            }
            catch(Exception exe)
            {
                Console.WriteLine(exe.ToString());
                return null;
            }
        }

        public async Task<bool> CreateUserAsync(string username, string password)
        {
            string query = "INSERT (username, password) INTO users VALUES (@username, @hash)";
            object paramsObj = new { username,  hash = BCrypt.Net.BCrypt.HashPassword(password, 12)};

            return await _connection.ExecuteAsync(query, paramsObj) > 0;
        }

        public async Task<bool> CreateGroupAsync(string name)
        {
            string query = "INSERT (name) INTO groups VALUES (@name)";
            object paramsObj = new { name };

            return await _connection.ExecuteAsync(query, paramsObj) > 0;
        }

        public Task<bool> AssignUserToGroupAsync(int uid, int gid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateTaskAsync(string description, int gid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTaskStatusAsync(int tid, Models.TaskStatus newStatus)
        {
            throw new NotImplementedException();
        }
    }
}
