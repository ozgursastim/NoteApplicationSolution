using NoteApplication.Common.Helper;
using NoteApplication.DataAccessLayer.EntityFramework;
using NoteApplication.Entities;
using NoteApplication.Entities.Messages;
using NoteApplication.Entities.ValueObjects;
using System;

namespace NoteApplication.BusinessLayer
{
    public class NoteUserManager
    {
        private Repository<NoteUser> repositoryUser = new Repository<NoteUser>();

        public BusinessLayerResult<NoteUser> RegisterUser(RegisterViewModel data)
        {
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();
            NoteUser noteUser = repositoryUser.Find(x => x.Username == data.Username || x.Email == data.Email);

            if (noteUser != null)
            {
                if (noteUser.Username == data.Username)
                    businessLayerResult.AddError(ErrorMessageCode.UsernameAlreadyExist, "Username is exist");
                if (noteUser.Email == data.Email)
                    businessLayerResult.AddError(ErrorMessageCode.EmailAlreadyExist, "Email is exist");
            }
            else
            {
                int databaseResult = repositoryUser.Insert(new NoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });

                if (databaseResult > 0)
                {
                    businessLayerResult.Result = repositoryUser.Find(x => x.Username == data.Username && x.Email == data.Email);


                    // Send activate email
                    // businessLayerResult.Result.ActivateGuid == 
                    string webSiteUri = ConfigHelper.Get<string>("WebSiteUri");
                    string activateUri = $"{webSiteUri}/Home/UserActivate/{businessLayerResult.Result.ActivateGuid}";
                    string mailBody = ($"Hello {businessLayerResult.Result.Name} {businessLayerResult.Result.Surname}, <br><br> To activate your account click <a href='{activateUri} target='_blank'> link</a>");
                    MailHelper.SendMail(mailBody, businessLayerResult.Result.Email, "Note Application Account Activate");
                }

            }

            return businessLayerResult;
        }
        public BusinessLayerResult<NoteUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();
            NoteUser noteUser = repositoryUser.Find(x => x.Username == data.Username && x.Password == data.Password);

            businessLayerResult.Result = noteUser;

            if (noteUser != null)
            {
                if (!noteUser.IsActive)
                    businessLayerResult.AddError(ErrorMessageCode.UserInactive, "User is inactive. Check your email for activate");
            }
            else
            {
                businessLayerResult.AddError(ErrorMessageCode.UsernameOrPasswordWrong, "Invalid user or password");
            }

            return businessLayerResult;
        }

        public BusinessLayerResult<NoteUser> ActivateUser(Guid ActivateGuid)
        {
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();
            businessLayerResult.Result = repositoryUser.Find(x => x.ActivateGuid == ActivateGuid);
            
            if (businessLayerResult.Result != null)
            {
                if (businessLayerResult.Result.IsActive)
                {
                    businessLayerResult.AddError(ErrorMessageCode.UserAlreadyActive, "User already active");
                    return businessLayerResult;
                }
                businessLayerResult.Result.IsActive = true;
                repositoryUser.Update(businessLayerResult.Result);
            }
            else
            {
                businessLayerResult.AddError(ErrorMessageCode.UserActivateIdInvalid, "Invalid activate id");
                return businessLayerResult;
            }

            return businessLayerResult;
        }
    }
}
