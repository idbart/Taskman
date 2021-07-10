using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskmanWebApp.Models;
using TaskmanWebApp.Scripts.Interfaces;

namespace TaskmanWebApp.Controllers
{
    [Authorize]
    public class GroupsController : ApiBaseController
    {
        private readonly IDataAccess _dataAccess;
        public GroupsController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // endpoint for the client to get model for thier group
        [HttpGet("")]
        public async Task<GroupModel> GetClientGroupAsync()
        {
            UserModel client = await _dataAccess.GetUserAsync(int.Parse(HttpContext.User.FindFirst("id").Value));
            GroupModel clientGroup = await _dataAccess.GetGroupAsync(client.gid);

            // if the user has no group, return a 404
            if (clientGroup == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }
            else
            {
                return clientGroup;
            }
        }

        // get data about a group
        [HttpGet("{id:int}")]
        public async Task<GroupModel> GetGroupAsync(int gid)
        {
            // get the goup data from the db
            GroupModel group = await _dataAccess.GetGroupAsync(gid);

            if (group != null)
            {
                // check if the currently authenticated user is in this group
                if (group.HasUser(int.Parse(HttpContext.User.FindFirst("id").Value)))
                {
                    return group;
                }
                else
                {
                    // if this user is not in the group, dont let them see the tasks
                    group.tasks = null;
                    return group;
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 404;
                return new GroupModel() { name = "none" };
            }
        }

        // search for groups
        [HttpGet("{query}")]
        public async Task<IEnumerable<GroupModel>> SearchForGroup(string query)
        {

        }

        // get the tasks for a group
        [HttpGet("{id:int}/tasks")]
        public async Task<IEnumerable<TaskModel>> GetGroupTasksAsync(int id)
        {
            // for now just get the group model and return the tasks if the authenticated user is in the group
            // maybe later add a method signature to the IDataAccess interface that allows the retrival of just the task list for a specified group
            GroupModel group = await _dataAccess.GetGroupAsync(id);
            if (group.HasUser(int.Parse(HttpContext.User.FindFirst("id").Value)))
            {
                return group.tasks;
            }
            else
            {
                HttpContext.Response.StatusCode = 403;
                return null;
            }
        }

        [HttpPost("{id:int}/tasks/create")]
        public async Task<TaskModel> CreateNewTaskAsync(int id, [FromBody]TaskModel newTask)
        {
            // check if the user in in the group
            GroupModel group = await _dataAccess.GetGroupAsync(id);
            if (group.HasUser(int.Parse(HttpContext.User.FindFirst("id").Value)))
            {
                // check if the user provided a description for the new task
                if (!String.IsNullOrEmpty(newTask.description))
                {
                    // make sure the db actually creates a new resource and then return the new task data as provied by the user
                    // ideally this should return the newly created resource in full, maybe ill do somthing about that later
                    if (await _dataAccess.CreateTaskAsync(newTask.description, id))
                    {
                        return newTask;
                    }
                    else
                    {
                        HttpContext.Response.StatusCode = 500;
                        return null;
                    }
                }
                else
                {
                    HttpContext.Response.StatusCode = 400;
                    return new TaskModel() { description = "description cannot be empty"  };
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 403;
                return null;
            }
        }
    }
}
