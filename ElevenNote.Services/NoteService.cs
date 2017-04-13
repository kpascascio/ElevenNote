using ElevenNote.Data;
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

        //the Guid is for the user that we want to pass in and store information for. 

        //the web layer has an expectation to the service to create notes based on the user that is interacting 
        //with it.
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
            //applicationdbcontext is a stream, we want to access the stream so we call it then it opens
            //but as a clean up we also need to close the stream, so it cannot belong in the constructor
            //keep it constrained within a method and a using statment we can limit the time its open
            // also using allows it to be disposed after it's done. 
            using(var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx //this is just an arbitrary name that is short for context 
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
                                IsStarred = e.IsStarred,
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

        public NoteDetail GetNoteById(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                //We want to just pull one out so we use Single vs Where, which is a enumberable and gives us more than we need
                //We dont need to call toArray because Single does that for us!
                //We can use the OwnerId because in this context we are still in the database
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.OwnerId == _userId && e.NoteId == noteId);

                return
                    new NoteDetail
                    {
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc

                    };
            }
        } 

        //EF(Entity Framework) wants to track this information, we need to get the single note id from the passed in model
        //This looks just like the create method.
        public bool UpdateNote(NoteEdit model)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                ctx
                   .Notes
                   .Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        } 

        public bool DeleteNote(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                ctx
                   .Notes
                   .Single(e => e.OwnerId == _userId && e.NoteId == noteId);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
