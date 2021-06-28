using NoteApplication.BusinessLayer.Abstract;
using NoteApplication.Entities;
using System.Linq;

namespace NoteApplication.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();

            foreach (var note in category.Notes.ToList())
            {
                foreach (var like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }
                foreach (var comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
            }
            return base.Delete(category);
        }
    }
}
