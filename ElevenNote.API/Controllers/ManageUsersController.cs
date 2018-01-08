using ElevenNote.Data;
using ElevenNote.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.API.Controllers
{
    [Authorize(Roles = SecurityRoles.Admin)]
    public class ManageUsersController : ApiController
    {
        public IHttpActionResult Get()
        {
            var service = ManageUsersService();
            return Ok(service.GetUsers());
        }

        private ManageUsers ManageUsersService()
        {
            var service = new ManageUsers();
            return service;
        }
    }
}
