using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.Web.Controllers.WebApi
{
    //we don't want just anyone to change our notes, so we want to only allow autourize users.
    //this will not redirect to the login page, it will just say unauthourize.
    [Authorize]

    //one thing that is different about webapi is the routing! we need to place attribute base routing
    // this sets up a separate url in the application to see the api route
    [RoutePrefix("api/Note")]
    public class NoteController : ApiController
    {
        //notice the Note controller is inheriting from the ApiController, which in this case is IHttpContent,and IDisposable
        [Route("{id}/Star")]
        [HttpPut]
        public bool ToggleStarOn(int id) => SetStarState(id, true);

        
        [Route("{id}/Star")]
        [HttpDelete]
        public bool ToggleStarOff(int id) => SetStarState(id, false);
        
        private bool SetStarState(int noteId, bool newState)
        {
            NoteService service = CreateNoteService();

            var detail = service.GetNoteById(noteId);

            var updatedNote =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content,
                    IsStarred = newState
                };

            return service.UpdateNote(updatedNote);
        }

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}
