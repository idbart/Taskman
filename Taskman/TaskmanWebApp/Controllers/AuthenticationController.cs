using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskmanWebApp.Models;

namespace TaskmanWebApp.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        [HttpGet("login")]
        public UserModel Login()
        {
            throw new NotImplementedException();
        }
    }
}
