using NoteApplication.BusinessLayer;
using NoteApplication.Entities;
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
            return View();
        }
    }
}