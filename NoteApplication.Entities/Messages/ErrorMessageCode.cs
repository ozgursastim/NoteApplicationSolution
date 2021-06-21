using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExist = 100,
        EmailAlreadyExist = 101,
        UserInactive = 200,
        UsernameOrPasswordWrong = 201,
        CheckYourEmail = 202,
        UserAlreadyActive = 203,
        UserActivateIdInvalid = 204
    }
}
