using NoteApplication.BusinessLayer;
using NoteApplication.BusinessLayer.Result;
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
        private NoteManager _noteManager = new NoteManager();
        private NoteUserManager _noteUserManager = new NoteUserManager();
        private CategoryManager _categoryManager = new CategoryManager();

        public ActionResult Index()
        {

            //BusinessLayer.Test test = new BusinessLayer.Test();

            return View(_noteManager.ListIQueryable().OrderByDescending(x => x.ModifiedDate).ToList());
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = _categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            return View("Index", category.Notes.OrderByDescending(x => x.ModifiedDate).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index", _noteManager.ListIQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            NoteUser currentUser = Session["login"] as NoteUser;

            BusinessLayerResult<NoteUser> businessLayerResult = _noteUserManager.GetUserById(currentUser.Id);
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

            BusinessLayerResult<NoteUser> businessLayerResult = _noteUserManager.GetUserById(currentUser.Id);
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
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
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

                BusinessLayerResult<NoteUser> businessLayerResult = _noteUserManager.UpdateProfile(noteUser);

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
            return View(noteUser);
        }

        public ActionResult RemoveProfile()
        {
            NoteUser currentUser = Session["login"] as NoteUser;
            BusinessLayerResult<NoteUser> businessLayerResult = _noteUserManager.RemoveUserById(currentUser.Id);
            ErrorViewModel errorViewModel;

            if (businessLayerResult.Errors.Count > 0)
            {
                errorViewModel = new ErrorViewModel()
                {
                    Title = "Profile didn't remove",
                    Items = businessLayerResult.Errors,
                    RedirectUrl = "/Home/ShowProfile"
                };

                return View("Error", errorViewModel);
            }

            Session.Clear();
            return RedirectToAction("Index");
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
                BusinessLayerResult<NoteUser> businessLayerResult = _noteUserManager.LoginUser(model);

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
            if (ModelState.IsValid)
            {
                BusinessLayerResult<NoteUser> businessLayerResult = _noteUserManager.RegisterUser(model);
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
            BusinessLayerResult<NoteUser> businessLayerResult = _noteUserManager.ActivateUser(id);
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