using NoteApplication.BusinessLayer;
using NoteApplication.Entities;
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

            return View(noteManager.GetAllNotes());
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
            return View("Index", category.Notes);
        }
    }
}