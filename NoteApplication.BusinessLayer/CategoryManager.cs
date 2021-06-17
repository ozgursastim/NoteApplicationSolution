using NoteApplication.DataAccessLayer.EntityFramework;
using NoteApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication.BusinessLayer
{
    public class CategoryManager
    {
        private Repository<Category> repositoryCategory = new Repository<Category>();
        public List<Category> GetAllCategories()
        {
            return repositoryCategory.List();
        }
        public Category GetCategoryById(int id)
        {
            return repositoryCategory.Find(x => x.Id == id);
        }
    }
}
