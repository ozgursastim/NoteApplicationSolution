using NoteApplication.DataAccessLayer.EntityFramework;
using NoteApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication.BusinessLayer
{
    public class NoteManager
    {
        private Repository<Note> repositoryNote = new Repository<Note>();

        public List<Note> GetAllNotes()
        {
            return repositoryNote.List();
        }
    }
}
