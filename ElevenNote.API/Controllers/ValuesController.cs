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
            var service = new NoteService(Guid.Parse("6698de7b-705b-4dee-8d2b-21bfe5091cd6"));
            var data = service.GetNotes();

            return Ok(data);
        }
    }
}
