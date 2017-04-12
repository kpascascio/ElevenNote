﻿using ElevenNote.Models;
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
            var model = new NoteListItem[0];
        
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
            //while blank we are going to set a break point to check out the data that is being 
            //passed. 
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}