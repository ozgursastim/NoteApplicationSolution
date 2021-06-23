using NoteApplication.BusinessLayer;
using NoteApplication.Entities;
using NoteApplication.Entities.Messages;
using NoteApplication.Entities.ValueObjects;
using NoteApplication.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NoteApplication.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            //BusinessLayer.Test test = new BusinessLayer.Test();

            NoteManager noteManager = new NoteManager();

            return View(noteManager.GetAllNotes().OrderByDescending(x => x.ModifiedDate).ToList());
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager categoryManager = new CategoryManager();
            Category category = categoryManager.GetCategoryById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            return View("Index", category.Notes.OrderByDescending(x => x.ModifiedDate).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager noteManager = new NoteManager();

            return View("Index", noteManager.GetAllNotes().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            NoteUser currentUser = Session["login"] as NoteUser;
            NoteUserManager noteUserManager = new NoteUserManager();
            BusinessLayerResult<NoteUser> businessLayerResult = noteUserManager.GetUserById(currentUser.Id);
            ErrorViewModel errorViewModel;

            if (businessLayerResult.Errors.Count > 0)
            {
                errorViewModel = new ErrorViewModel()
                {
                    Title = "Incorrect Proccess",
                    Items = businessLayerResult.Errors
                };

                return View("Error", errorViewModel);
            }
            return View(businessLayerResult.Result);
        }

        public ActionResult EditProfile()
        {
            NoteUser currentUser = Session["login"] as NoteUser;
            NoteUserManager noteUserManager = new NoteUserManager();
            BusinessLayerResult<NoteUser> businessLayerResult = noteUserManager.GetUserById(currentUser.Id);
            ErrorViewModel errorViewModel;

            if (businessLayerResult.Errors.Count > 0)
            {
                errorViewModel = new ErrorViewModel()
                {
                    Title = "Incorrect Proccess",
                    Items = businessLayerResult.Errors
                };

                return View("Error", errorViewModel);
            }
            return View(businessLayerResult.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(NoteUser noteUser, HttpPostedFileBase ProfileImage)
        {
            if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"
                ))
            {
                string filename = $"user_{noteUser.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                noteUser.ProfileImageFilename = filename;
            }

            NoteUserManager noteUserManager = new NoteUserManager();
            BusinessLayerResult<NoteUser> businessLayerResult = noteUserManager.UpdateProfile(noteUser);

            if (businessLayerResult.Errors.Count > 0)
            {
                ErrorViewModel errorViewModel = new ErrorViewModel()
                {
                    Items = businessLayerResult.Errors,
                    Title = "Profile could't update",
                    RedirectUrl = "/Home/EditProfile"
                };

                return View("Error", errorViewModel);
            }

            Session["login"] = businessLayerResult.Result;

            return RedirectToAction("ShowProfile");
        }

        public ActionResult RemoveProfile()
        {
            return View();
        }
        public ActionResult Login()
        {
            LoginViewModel md = new LoginViewModel();
            md.Username = "";
            md.Password = "";
            return View(md);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                NoteUserManager noteUserManager = new NoteUserManager();
                BusinessLayerResult<NoteUser> businessLayerResult = noteUserManager.LoginUser(model);

                if (businessLayerResult.Errors.Count > 0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }
                
                Session["login"] = businessLayerResult.Result;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            RegisterViewModel md = new RegisterViewModel();
            md.Username = "";
            md.Email = "";
            md.Password = "";
            md.RePassword = "";
            return View(md);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            NoteUser noteUser = new NoteUser();

            if (ModelState.IsValid)
            {
                NoteUserManager noteUserManager = new NoteUserManager();
                BusinessLayerResult<NoteUser> businessLayerResult = noteUserManager.RegisterUser(model);
                if (businessLayerResult.Errors.Count > 0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }

                OkViewModel okViewModel = new OkViewModel()
                {
                    Title = "Register success",
                    RedirectUrl = "/Home/Login"
                };
                okViewModel.Items.Add("You have been registered. Check your email");
                return RedirectToAction("Ok", okViewModel);
            }

            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            NoteUserManager noteUserManager = new NoteUserManager();
            BusinessLayerResult<NoteUser> businessLayerResult = noteUserManager.ActivateUser(id);
            ErrorViewModel errorViewModel;
            OkViewModel okViewModel;

            if (businessLayerResult.Errors.Count > 0)
            {
                errorViewModel = new ErrorViewModel()
                {
                    Title = "Invalid Proccess",
                    Items = businessLayerResult.Errors
                };
                
                return View("Error", errorViewModel);
            }

            okViewModel = new OkViewModel()
            {
                Title = "You have been activated"
            };
            okViewModel.Items.Add("Thank you. You have been activated");
            return RedirectToAction("Ok", okViewModel);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }
    }
}