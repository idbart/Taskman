using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskmanWebApp.Models;
using TaskmanWebApp.Scripts.Interfaces;
using BCrypt.Net;
using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace TaskmanWebApp.Scripts
{
    public class LocalDataAccess : IDataAccess
    {
        private IDbConnection _connection;

        public LocalDataAccess(IConfiguration config)
        {
            try
            {
                _connection = new SqliteConnection(config.GetConnectionString("Local"));
            }
            catch(Exception exe)
            {
                Console.WriteLine(exe.ToString());
                _connection = null;
            }
        }

        public Task<GroupModel> GetGroupAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GroupModel> GetGroupAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<TaskModel> GetTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetUserAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetUserAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateGroupAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
