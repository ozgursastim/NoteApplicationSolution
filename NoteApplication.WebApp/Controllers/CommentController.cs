using System.Net;
using System.Web.Mvc;
using NoteApplication.BusinessLayer;
using NoteApplication.Entities;

namespace NoteApplication.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private CommentManager _commentManager = new CommentManager();

        public ActionResult Index()
        {
            return View(_commentManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _commentManager.Find(x => x.Id == id.Value);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            ModelState.Remove("CreatedDate");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                _commentManager.Insert(comment);
                return RedirectToAction("Index");
            }

            return View(comment);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _commentManager.Find(x => x.Id == id.Value);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            ModelState.Remove("CreatedDate");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Comment databaseComment = _commentManager.Find(x => x.Id == comment.Id);
                databaseComment.Text = comment.Text;
                _commentManager.Update(databaseComment);

                return RedirectToAction("Index");
            }
            return View(comment);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _commentManager.Find(x => x.Id == id.Value);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = _commentManager.Find(x => x.Id == id);
            _commentManager.Delete(comment);
            return RedirectToAction("Index");
        }
    }
}
