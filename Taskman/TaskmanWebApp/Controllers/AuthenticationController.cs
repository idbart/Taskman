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
            // test the password (this will need to be hashed for comparison)
            if(user != null && claim.password == user.password)
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
    }
}
