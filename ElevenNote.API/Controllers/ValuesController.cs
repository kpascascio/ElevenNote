using ElevenNote.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.API.Controllers
{
    public class ValuesController : ApiController
    {
        public IHttpActionResult Get()
        {
            var service = new NoteService(Guid.Parse("52cfcdfc-4a29-49d2-abb2-b38822b69b12"));
            var data = service.GetNotes();
            var service22 = new AdminService(Guid.Parse("52cfcdfc-4a29-49d2-abb2-b38822b69b12"));

            service22.CheckAdmin();
            return Ok(data);
        }
    }
}
