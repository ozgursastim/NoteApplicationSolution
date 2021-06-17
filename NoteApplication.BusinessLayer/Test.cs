using NoteApplication.DataAccessLayer.EntityFramework;
using NoteApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            //DataAccessLayer.DatabaseContext databaseContext = new DataAccessLayer.DatabaseContext();
            //databaseContext.Categories.ToList();
            Repository<Category> repo = new Repository<Category>();
            List<Category> categories = repo.List();
        }
    }
}
