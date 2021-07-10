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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TaskmanWebApp.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IDataAccess _dataAccess;
        private readonly IWebHostEnvironment _env;

        public AuthenticationController(IDataAccess dataAccess, IWebHostEnvironment env)
        {
            _dataAccess = dataAccess;
            _env = env;
        }

        [Authorize]
        [HttpGet("/api/id")]
        public async Task<UserModel> GetSelfAsync()
        {
            Console.WriteLine("user getiing self");

            // return the client's user model without pasword
            UserModel user = await _dataAccess.GetUserAsync(int.Parse(HttpContext.User.FindFirstValue("id")));

            user.password = null;
            return user;
        }

        [HttpPost("/api/login")]
        public async Task<UserModel> LoginAsync([FromBody]UserModel claim)
        {
            // look for the user in the db
            UserModel user = await _dataAccess.GetUserAsync(claim.username);

            // if the db returns nothing, do the same to the client beacuse screw that guy
            if (user != null)
            {
                // test the password
                bool passwordMatch = false;
                try
                {
                    passwordMatch = BCrypt.Net.BCrypt.Verify(claim.password, user.password);
                }
                catch(Exception e)
                {
                    HttpContext.Response.StatusCode = 500;
                    return null;
                }

                if (passwordMatch)
                {
                    // create new user principal and sign in the user
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("id", user.id.ToString()));
                    claims.Add(new Claim("username", user.username));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Default");
                    ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity[] { identity });

                    await HttpContext.SignInAsync(principal);

                    // if the sign in is sucessfull, return a new UserModel without the password
                    user.password = null;
                    return user;
                }
                else
                {
                    HttpContext.Response.StatusCode = _env.IsDevelopment() ? 401 : 404;
                    return null;
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }
        }

        // endpoint for the user to logout
        [HttpPost("/api/logout")]
        public async Task LogoutAsync()
        {
            await HttpContext.SignOutAsync();
        }

        // endpoint for a user to create an account
        [HttpPost("/api/signup")]
        public async Task<UserModel> SignUpAsync([FromBody]UserModel newUser)
        {
            // return if the requet is incompleate
            if (string.IsNullOrEmpty(newUser.username) || string.IsNullOrEmpty(newUser.password))
            {
                HttpContext.Response.StatusCode = 400;
                return new UserModel() { username = string.IsNullOrEmpty(newUser.username) ? "username is required" : "", password = string.IsNullOrEmpty(newUser.password) ? "password is required" : "" };
            }

            // check if a user by the same name already exists
            if (await _dataAccess.GetUserAsync(newUser.username) == null)
            {
                if (await _dataAccess.CreateUserAsync(newUser.username, newUser.password))
                {
                    UserModel user = await _dataAccess.GetUserAsync(newUser.username);
                    user.password = null;

                    return user;
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
                return new UserModel() { username = "user already exists" };
            }
        }
    }
}
