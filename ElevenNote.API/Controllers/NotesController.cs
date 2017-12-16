using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.API.Controllers
{
    //We need to make sure that any request to this controller is authorized. If this fails we will get a 401 message
    [Authorize]
    public class NotesController : ApiController
    {
        //this is going return a 200 Ok message, don't be confused with it having 'GetAll' in the URL
        //the interface IHttpActionResult is very importabt to learn 
        public IHttpActionResult Get()
        {
            //we first need the users id 
            var svc = GetNotesService();
            var notes = svc.GetNotes();

            return Ok(notes);
        }


        public IHttpActionResult Get(int id)
        {
            //we first need the users id 
            var svc = GetNotesService();
            var note = svc.GetNoteById(id);

            if (note == null) return NotFound();

            return Ok(note);
        }

        public IHttpActionResult Post(NoteCreate note)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var svc = GetNotesService();

            if (!svc.CreateNote(note)) return InternalServerError();

            return Ok(svc.CreateNote(note));
        }

        public IHttpActionResult Put(NoteEdit note)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var svc = GetNotesService();
            

            var noteById = svc.GetNoteById(note.NoteId);
            if (noteById == null) return NotFound();

            return Ok(svc.UpdateNote(note));
        }

        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var svc = GetNotesService();

            var note = svc.GetNoteById(id);
            if (note == null) return NotFound();

            return Ok(svc.DeleteNote(id));
        }


        private NoteService GetNotesService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}
