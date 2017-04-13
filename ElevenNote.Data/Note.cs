using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Data
{
    //the represents the data that is stored in our database. 
    //Enitity is reading this class and creates a table from this model.
    //poco == plain old clr object
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        //Identifies the note 
        //Globalling unique identifier == Massive infustructor
        // Identity Framework uses GUIDS for it's users so we are using a identifiers 
        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        //always makes sure that false is in the db when we start
        [DefaultValue(false)]
        public bool IsStarred { get; set; }

        //timezones will be the bane of your existance , you need to standarize on a timezone
        // which will be UTC == universial time coordinated
        // DateTimeOffset carries along time zone information
        [Required]
        public DateTimeOffset CreatedUtc { get; set; }

        //are value types not reference types
        public DateTimeOffset? ModifiedUtc { get; set; }

        

    }
}
