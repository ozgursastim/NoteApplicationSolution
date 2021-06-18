using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoteApplication.WebApp.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Username"), Required(ErrorMessage = "{0} is Required")]
        public string Username { get; set; }

        [DisplayName("Password"), Required(ErrorMessage = "{0} is Required"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}