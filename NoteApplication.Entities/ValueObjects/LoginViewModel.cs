using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoteApplication.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("Username"), Required(ErrorMessage = "{0} is Required")]
        public string Username { get; set; }

        [DisplayName("Password"), Required(ErrorMessage = "{0} is Required"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}