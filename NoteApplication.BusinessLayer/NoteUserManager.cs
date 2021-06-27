using NoteApplication.BusinessLayer.Abstract;
using NoteApplication.BusinessLayer.Result;
using NoteApplication.Common.Helper;
using NoteApplication.DataAccessLayer.EntityFramework;
using NoteApplication.Entities;
using NoteApplication.Entities.Messages;
using NoteApplication.Entities.ValueObjects;
using System;

namespace NoteApplication.BusinessLayer
{
    public class NoteUserManager : ManagerBase<NoteUser>
    {
        public BusinessLayerResult<NoteUser> RegisterUser(RegisterViewModel data)
        {
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();
            NoteUser noteUser = Find(x => x.Username == data.Username || x.Email == data.Email);

            if (noteUser != null)
            {
                if (noteUser.Username == data.Username)
                    businessLayerResult.AddError(ErrorMessageCode.UsernameAlreadyExist, "Username is exist");
                if (noteUser.Email == data.Email)
                    businessLayerResult.AddError(ErrorMessageCode.EmailAlreadyExist, "Email is exist");
            }
            else
            {
                int databaseResult = Insert(new NoteUser()
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
                    businessLayerResult.Result = Find(x => x.Username == data.Username && x.Email == data.Email);


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

        public BusinessLayerResult<NoteUser> GetUserById(int id)
        {
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();
            businessLayerResult.Result = Find(x => x.Id == id);

            if (businessLayerResult.Result == null)
            {
                businessLayerResult.AddError(ErrorMessageCode.UserNotExist, "User not exists");
            }

            return businessLayerResult;
        }

        public BusinessLayerResult<NoteUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();
            NoteUser noteUser = Find(x => x.Username == data.Username && x.Password == data.Password);

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
            businessLayerResult.Result = Find(x => x.ActivateGuid == ActivateGuid);
            
            if (businessLayerResult.Result != null)
            {
                if (businessLayerResult.Result.IsActive)
                {
                    businessLayerResult.AddError(ErrorMessageCode.UserAlreadyActive, "User already active");
                    return businessLayerResult;
                }
                businessLayerResult.Result.IsActive = true;
                Update(businessLayerResult.Result);
            }
            else
            {
                businessLayerResult.AddError(ErrorMessageCode.UserActivateIdInvalid, "Invalid activate id");
                return businessLayerResult;
            }

            return businessLayerResult;
        }

        public BusinessLayerResult<NoteUser> UpdateProfile(NoteUser data)
        {
            NoteUser databaseNoteUser = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();

            if (databaseNoteUser != null && databaseNoteUser.Id != data.Id)
            {
                if (databaseNoteUser.Username == data.Username)
                {
                    businessLayerResult.AddError(ErrorMessageCode.UsernameAlreadyExist, "User exist");
                }
                if (databaseNoteUser.Email == data.Email)
                {
                    businessLayerResult.AddError(ErrorMessageCode.EmailAlreadyExist, "Email exist");
                }

                return businessLayerResult;
            }

            businessLayerResult.Result = Find(x => x.Id == data.Id);
            businessLayerResult.Result.Name = data.Name;
            businessLayerResult.Result.Surname = data.Surname;
            businessLayerResult.Result.Email = data.Email;
            businessLayerResult.Result.Password = data.Password;
            businessLayerResult.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                businessLayerResult.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (Update(businessLayerResult.Result) == 0)
            {
                businessLayerResult.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profile could not update");
            }

            return businessLayerResult;

        }

        public BusinessLayerResult<NoteUser> RemoveUserById(int id)
        {
            NoteUser databaseNoteUser = Find(x => x.Id == id);
            BusinessLayerResult<NoteUser> businessLayerResult = new BusinessLayerResult<NoteUser>();

            if (databaseNoteUser != null)
            {
                if (Delete(databaseNoteUser) == 0)
                {
                    businessLayerResult.AddError(ErrorMessageCode.ProfileCouldNotDelete, "Profile could not remove");
                }
            }
            else
            {
                businessLayerResult.AddError(ErrorMessageCode.UserNotExist, "User not exist");
            }
            return businessLayerResult;
        }
    }
}
