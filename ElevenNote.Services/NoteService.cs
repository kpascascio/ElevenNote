﻿using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    //make sure this class is made public 
    public class NoteService
    {
        //backing field
        private readonly Guid _userId;
        //make sure we let NoteService knowswho is currently using the application

        public NoteService(Guid userId)
        {
            //if we create a new instance of note service we need to give it an id.
            _userId = userId;
        }


        //wether or not we can write to the DB, For Error messages to see if the correct things are saved
        //NoteCreate pushes data down to the db.
        public bool CreateNote(NoteCreate model)
        {
            //shorthand style of adding properties to a instance of an object
            var entity = new Note
            {
                OwnerId = _userId,
                Title = model.Title,
                Content = model.Content, 
                CreatedUtc = DateTimeOffset.UtcNow
            };

            //DB only have a set number of connections to them, so we need to close the context.

            using(var ctx = new ApplicationDbContext())
            {
                //we need to bring in asp.net identity nuget package to the project
                // this tells EF to start tracking this element 
                ctx.Notes.Add(entity);

                //remember we are trying to retrun a bool that 1 is the row
                //number of rows that were effected in the database. So there should be one row that updates
                //in the DB, if it's 0 then nothing if it's more than 1 then something went wrong!
                return ctx.SaveChanges() == 1; 
            }

        }

        //getting the data out from NoteCreate from our Database
        public IEnumerable<NoteListItem> GetNotes()
        {
            //applicationdbcontext needs to be short life, so it cannot belong in the constructor
            //keep it constrained within a method and a using statment we can limit the time its open
            // also using allows it to be disposed after it's done. 
            using(var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId)
                        //putting this in a way that the view can recieve it.
                        .Select
                        (
                            e => new NoteListItem
                            {
                                //these are the fields that are present on the NoteList page!
                                NoteId = e.NoteId,
                                Title = e.Title,
                                CreatedUtc = e.CreatedUtc
                            }
                        );

                //deffered execution, the quesry doesn't run unless you iterate over it. 
                //I know how to get this data but I havent gotten it when we only try to return the query.
                // there is another step...

                //this is that next step: force this to execute this here when we called the 
                //toArray method on the query in our return object.
                return query.ToArray();
            }
        } 
    }
}