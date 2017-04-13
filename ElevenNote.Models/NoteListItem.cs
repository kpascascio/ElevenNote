using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteListItem
    {
        /*
         *  Make you're class public so that it can be used throughout the application 
         * 
         * 
         * poco == plain old clr object
         */

        public int NoteId { get; set; }
        //We need to tell the webside which note we want to edit, that's why we build a NoteId


        public string Title { get; set; }

        [UIHint("Starred")]
        public bool IsStarred { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }
        //we need to add in the reference but it's present in the MVC, VS is smarrt enough to help us out wuth that 

        public override string ToString() => $"[{NoteId}] title";
    }
}
