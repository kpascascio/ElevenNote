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
        // GET api/values
        [HttpGet]
        public IHttpActionResult Get()
        {
            //return new string[] { "value1", "value2" };
            var service = new NoteService(Guid.Parse("6698de7b-705b-4dee-8d2b-21bfe5091cd6"));
            var data = service.GetNotes();

            return Ok(data);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
