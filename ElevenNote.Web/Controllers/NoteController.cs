using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Web.Controllers
{
    //we want to be able to only have a logged in user see these pages 
    // let's let MVC framework do this work with this annotation
    // this says when you get a request make sure that the user is authenticated, if not direct ot login page
    [Authorize]
    public class NoteController : Controller
    {
        public ActionResult Index()
        {
            //bringing in the model from the model layer
            //Notice that we passed in the array
            // think of how we use string[] 
            // NoteListItem is a data type and we want to pass in an empty array of zero to have something to iterate over.
            //var model = new NoteListItem[0]; we dont need this anymore after we created our service 

            //GetUserId is an extension so we need to do a ctrl dot(.)
            var service = CreateNoteService();

            // model is a Ienumerable for data from our service #mindblown
            var model = service.GetNotes();

            return View(model);
        }

        //We aren't giving any data, we are going to be creating data with this
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            //this check if the data is incorrect and the server is bueno
            if (!ModelState.IsValid) return View(model);

            var service = CreateNoteService();


            //This if handles if everything is bueno
            if (service.CreateNote(model))
            {
                //we need to share this data to another view, to be available by the index

                //ViewBag.SaveResult = "Your note was created";

                // this is a small wraper on top of the session
                // TempData works across views. 
                // TempData is a dictionary, so we create a key of SaveResults with the value of a string.
                TempData["SaveResult"] = "Your note was created";
                //when you read a value it removes it from the session, so its only temporarialy stored
                return RedirectToAction("Index");
            };

            //the if statement above happens after submission was valid 
            //this model state is allowing us to pass an error message to our view.
            // this is a great case if the server isn't responding, thats when it would reach this statement
            ModelState.AddModelError("", "Your note could not be created");
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateNoteService();
            var model = svc.GetNoteById(id);
            return View(model);
        }

        //lower case id is important because that the convention used in the route
        public ActionResult Edit(int id)
        {
            var svc = CreateNoteService();
            var detail = svc.GetNoteById(id);
            var model =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content
                };

            return View(model);
        }
        //we need to post the data from the edit page!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if(model.NoteId != id)
            {
                ModelState.AddModelError("", "Note ID is mis-matched");
                return View(model);
            }

            var service = CreateNoteService();
            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated";
                return RedirectToAction("Index");
            };

            TempData["SaveResult"] = "Your note was not updated";
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var svc = CreateNoteService();
            var model = svc.GetNoteById(id);
            return View(model);
        }

        [HttpPost]
        //this action overrides the name of the action method that it's called on
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var svc = CreateNoteService();
            var model = svc.DeleteNote(id);

            TempData["SaveResults"] = "Your note was deleted!";

            return RedirectToAction("Index");
        }

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}