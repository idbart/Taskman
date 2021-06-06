using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskmanWebApp.Models;
using TaskmanWebApp.Scripts.Interfaces;
using BCrypt.Net;

namespace TaskmanWebApp.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IDataAccess _dataAccess;

        public AuthenticationController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [HttpPost("/api/login")]
        public async Task<UserModel> Login([FromBody]UserModel claim)
        {
            // look for the user in the db
            UserModel user = await _dataAccess.GetUserAsync(claim.username);

            // if the db returns nothing, do the same to the client beacuse screw that guy
            // test the password
            if (user != null && BCrypt.Net.BCrypt.Verify(claim.password, user.password))
            {
                // create new user principal and sign in the user
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("id", user.id.ToString()));
                claims.Add(new Claim("username", user.username));

                ClaimsIdentity identity = new ClaimsIdentity(claims);
                ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity[] { identity });

                await HttpContext.SignInAsync(principal);

                // if the sign in is sucessfull, return a new UserModel wihout the password
                return new UserModel() { id = user.id, username = user.username };
            }
            else
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }
        }

        [HttpPost("/api/logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        [HttpPost("/api/signup")]
        public async Task<UserModel> SignUp([FromBody]UserModel newUser)
        {
            // return if the requet is incompleate
            if (string.IsNullOrEmpty(newUser.username) || string.IsNullOrEmpty(newUser.password))
            {
                HttpContext.Response.StatusCode = 402;
                return new UserModel() { username = string.IsNullOrEmpty(newUser.username) ? "username is required" : "", password = string.IsNullOrEmpty(newUser.password) ? "password is required" : "" };
            }

            // check if a user by the same name already exists
            if (await _dataAccess.GetUserAsync(newUser.username) == null)
            {
                if (await _dataAccess.CreateUserAsync(newUser.username, newUser.password))
                {
                    return await _dataAccess.GetUserAsync(newUser.username);
                }
                else
                {
                    HttpContext.Response.StatusCode = 402;
                    return null;
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 402;
                return new UserModel() { username = "user already exists" };
            }
        }
    }
}
