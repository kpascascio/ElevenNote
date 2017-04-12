using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteCreate
    {
        [Display(Name ="Add a Title")]
        [Required]
        [MinLength(2, ErrorMessage ="Please enter atleast 2 characters")]
        [MaxLength(100, ErrorMessage ="There are too many characters in this field")]
        public string Title { get; set; }

        [Required]
        [MaxLength(8000)]
        public string Content { get; set; }

        //this is just a method that is just going to return the title of a instance
        public override string ToString() => Title;
    }
}
