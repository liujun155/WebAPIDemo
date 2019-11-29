using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestWebApi.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        public string GetTestString()
        {
            return "HelloWorld";
        }
    }
}
