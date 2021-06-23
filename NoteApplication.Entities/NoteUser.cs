using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication.Entities
{
    [Table("NoteUsers")]
    public class NoteUser :EntityBase
    {
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(100)]
        public string Surname { get; set; }
        
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [StringLength(75)]
        public string ProfileImageFilename { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public Guid ActivateGuid { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
