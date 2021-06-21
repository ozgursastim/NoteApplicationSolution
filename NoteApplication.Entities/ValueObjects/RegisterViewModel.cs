using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteApplication.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Username"),
            Required(ErrorMessage = "{0} is Required"),
            StringLength(50, ErrorMessage = "{0} is max {1} characters")]
        public string Username { get; set; }

        [DisplayName("Email"),
            Required(ErrorMessage = "{0} is Required"),
            StringLength(100, ErrorMessage = "{0} is max {1} characters"),
            EmailAddress(ErrorMessage = "{0} is not valid email")]
        public string Email { get; set; }

        [DisplayName("Password"),
            Required(ErrorMessage = "{0} is Required"),
            DataType(DataType.Password),
            StringLength(100, ErrorMessage = "{0} is max {1} characters")]
        public string Password { get; set; }

        [DisplayName("RePassword"),
            Required(ErrorMessage = "{0} is Required"),
            DataType(DataType.Password),
            StringLength(100, ErrorMessage = "{0} is max {1} characters"),
            Compare("Password", ErrorMessage = "{0} is not equal to the {1} ")]
        public string RePassword { get; set; }
    }
}