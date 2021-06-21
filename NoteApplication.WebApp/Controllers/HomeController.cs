using NoteApplication.BusinessLayer;
using NoteApplication.Entities;
using NoteApplication.Entities.Messages;
using NoteApplication.Entities.ValueObjects;
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
                return RedirectToAction("RegisterOk");
            }

            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            NoteUserManager noteUserManager = new NoteUserManager();
            BusinessLayerResult<NoteUser> businessLayerResult = noteUserManager.ActivateUser(id);

            if (businessLayerResult.Errors.Count > 0)
            {
                TempData["errors"] = businessLayerResult.Errors;
                return RedirectToAction("UserActivateCancel");
            }

            return View();
        }

        public ActionResult UserActivateOk()
        {
            return View();
        }

        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObject> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObject>;
            }
            return View(errors);
        }
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }
    }
}